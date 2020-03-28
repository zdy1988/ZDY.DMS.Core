using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZDY.DMS;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.WorkFlowService.Events;
using ZDY.DMS.Services.WorkFlowService.DataObjects;
using ZDY.DMS.Models;
using ZDY.DMS.Tools;
using Zdy.Events;
using ZDY.DMS.StringEncryption;
using ZDY.DMS.Domain.Enums;
using ZDY.DMS.Domain.Events;
using ZDY.DMS.Services.WorkFlowService.ServiceContracts;
using ZDY.DMS.Querying.DataTableGateway;

namespace ZDY.DMS.Services.WorkFlowService.Implementation
{
    public class WorkFlowWorkingService : IWorkFlowWorkingService
    {
        private readonly IEventBus eventBus;
        private readonly IDataTableGateway adoNetDataTableGateway;
        private readonly IStringEncryption stringEncryption;
        private readonly IRepositoryContext repositoryContext;
        private readonly IRepository<Guid, User> userRepository;
        private readonly IRepository<Guid, UserGroup> userGroupRepository;
        private readonly IRepository<Guid, UserGroupMember> userGroupMemberRepository;
        private readonly IRepository<Guid, WorkFlow> workFlowRepository;
        private readonly IRepository<Guid, WorkFlowTask> workFlowTaskRepository;
        private readonly IRepository<Guid, WorkFlowInstance> workFlowInstanceRepository;


        private readonly AsyncLock execute_lock = new AsyncLock();

        public WorkFlowWorkingService(IRepositoryContext repositoryContext,
                                      IStringEncryption stringEncryption,
                                      IEventBus eventBus,
                                      IDataTableGateway adoNetDataTableGateway)
        {
            this.eventBus = eventBus;
            this.adoNetDataTableGateway = adoNetDataTableGateway;
            this.stringEncryption = stringEncryption;
            this.repositoryContext = repositoryContext;
            this.userRepository = repositoryContext.GetRepository<Guid, User>();
            this.userGroupRepository = repositoryContext.GetRepository<Guid, UserGroup>();
            this.userGroupMemberRepository = repositoryContext.GetRepository<Guid, UserGroupMember>();
            this.workFlowRepository = repositoryContext.GetRepository<Guid, WorkFlow>();
            this.workFlowTaskRepository = repositoryContext.GetRepository<Guid, WorkFlowTask>();
            this.workFlowInstanceRepository = repositoryContext.GetRepository<Guid, WorkFlowInstance>();
        }

        /// <summary>
        /// 创建一个流程实例，即发起一个流程
        /// </summary>
        /// <param name="instance"></param>
        public async Task StartUp(WorkFlowInstance instance)
        {
            await StartWorkFlowInstance(instance, GuidHelper.NewGuid());

            await this.repositoryContext.CommitAsync();
        }

        /// <summary>
        /// 处理流程
        /// </summary>
        /// <param name="execute">处理实体</param>
        /// <returns></returns>
        public async Task Execute(WorkFlowExecute execute)
        {
            //加载运行实例
            var instance = await workFlowInstanceRepository.FindAsync(t => t.Id == execute.InstanceId && t.IsDisabled == false);
            if (instance == null)
            {
                throw new InvalidOperationException("流程数据丢失");
            }
            execute.WorkFlowInstalled = WorkFlowAnalyzing.WorkFlowInstalledDeserialize(instance.FlowRuntimeJson);

            //获取当前步骤
            var current = await GetTaskAndStepForExecuting(execute);

            //必须签名
            if (current.Item2.SignatureType == (int)WorkFlowSignatureKinds.CommentAndSignature)
            {
                if (String.IsNullOrEmpty(execute.SignPassword))
                {
                    throw new InvalidOperationException("此步骤必须签名，请提供签名密钥");
                }
                else
                {
                    var user = await userRepository.FindAsync(t => t.Id == execute.Sender.Id);

                    if (user == null || user.Password != this.stringEncryption.Encrypt(execute.SignPassword))
                    {
                        throw new InvalidOperationException("密钥错误，签名失败");
                    }
                }
                execute.IsNeedSign = true;
            }
            else
            {
                execute.IsNeedSign = false;
            }

            if (string.IsNullOrEmpty(execute.Title))
            {
                execute.Title = current.Item1.Title;
            }

            using (await execute_lock.LockAsync())
            {
                switch (execute.ExecuteType)
                {
                    case WorkFlowExecuteKinds.Submit:
                        ExecuteSubmitBeforeEvent(current.Item1, current.Item2);
                        await ExecuteSubmit(execute, instance, current);
                        ExecuteSubmitAfterEvent(current.Item1, current.Item2);
                        break;
                    case WorkFlowExecuteKinds.Back:
                        ExecuteBackAfterEvent(current.Item1, current.Item2);
                        await ExecuteBack(execute, instance, current);
                        ExecuteBackAfterEvent(current.Item1, current.Item2);
                        break;
                    case WorkFlowExecuteKinds.Redirect:
                        await ExecuteRedirect(execute, instance, current);
                        break;
                    default:
                        throw new InvalidOperationException("流程处理方式出现错误");
                }
            }

            //提交事件
            eventBus.Commit();

            //更新实例中步骤信息
            await FreshenWorkFlowInstance(execute.InstanceId, current.Item1);
        }

        #region 流程实例相关

        /// <summary>
        /// 开启流程实例
        /// </summary>
        /// <returns></returns>
        private async Task<WorkFlowTask> StartWorkFlowInstance(WorkFlowInstance instance, Guid instanceGroupId)
        {
            if (instance.FlowId.Equals(default))
            {
                throw new InvalidOperationException("流程数据未找到，发起流程失败");
            }

            var workflowEntity = await workFlowRepository.FindAsync(t => t.Id == instance.FlowId && t.State == (int)WorkFlowState.Installed);

            if (workflowEntity == null)
            {
                throw new InvalidOperationException("流程数据未找到或流程未安装");
            }

            var workFlowInstalled = WorkFlowAnalyzing.WorkFlowInstalledDeserialize(workflowEntity.RuntimeJson);

            //设置开始节点和结束节点的一些默认规则
            foreach (var step in workFlowInstalled.Steps)
            {
                if (step.IsStart() || step.IsEnd())
                {
                    step.FlowControl = (int)WorkFlowControlKinds.System;  //系统控制流转
                    step.IsAllowRuntimeToSelect = false; //不允许选择
                    step.HandleTactic = (int)WorkFlowHandleTacticKinds.Independent;  //独立处理
                    step.HandlerType = (int)WorkFlowHandlerKinds.User;  //制定创建者为处理人
                    step.Handlers = instance.CreaterId.ToString();
                    step.BackTactic = (int)WorkFlowBackTacticKinds.AllowReturn; //允许退回上一步
                    step.BackType = (int)WorkFlowBackKinds.ToPrev;
                    step.CountersignatureTactic = (int)WorkFlowCountersignatureTacticKinds.NoCountersignature; // 不会签
                }
            }

            var flowRuntimeJson = JsonConvert.SerializeObject(workFlowInstalled);

            instance.Id = GuidHelper.NewGuid();
            instance.FlowName = workflowEntity.Name;
            instance.FlowRuntimeJson = flowRuntimeJson;
            instance.FlowDesignJson = workflowEntity.DesignJson;

            //创建实例
            await workFlowInstanceRepository.AddAsync(instance);

            //创建开始任务
            var execute = new WorkFlowExecute
            {
                WorkFlowInstalled = workFlowInstalled,
                Title = instance.Title,
                FlowId = instance.FlowId,
                FlowName = instance.FlowName,
                InstanceId = instance.Id,
                GroupId = instanceGroupId,
                Sender = new WorkFlowUser
                {
                    Id = instance.CreaterId,
                    Name = instance.CreaterName
                },
                CompanyId = instance.CompanyId
            };

            var firstTask = await CreateFirstTask(execute);

            //更新实例中步骤信息
            await FreshenWorkFlowInstance(instance.Id, firstTask);

            return firstTask;
        }

        /// <summary>
        /// 删除流程实例
        /// </summary>
        /// <returns></returns>
        private async Task RemoveWorkFlowInstance(Guid instanceId, Guid flowId, Guid instanceGroupId)
        {
            //删除实例
            var workFlowInstances = await workFlowInstanceRepository.FindAllAsync(t => t.Id == instanceId && t.FlowId == flowId);

            if (workFlowInstances.Count() > 0)
            {
                foreach (var instance in workFlowInstances)
                {
                    instance.IsDisabled = true;

                    await workFlowInstanceRepository.UpdateAsync(instance);
                }
            }

            //删除任务
            var workFlowTasks = await workFlowTaskRepository.FindAllAsync(t => t.InstanceId == instanceId && t.FlowId == flowId && t.GroupId == instanceGroupId);

            if (workFlowTasks.Count() > 0)
            {
                foreach (var task in workFlowTasks)
                {
                    task.IsDisabled = true;

                    await workFlowTaskRepository.UpdateAsync(task);
                }
            }
        }

        /// <summary>
        /// 结束流程实例
        /// </summary>
        /// <param name="instanceId"></param>
        /// <param name="flowId"></param>
        /// <param name="groupId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        private async Task FinishWorkFlowInstance(WorkFlowTask endStepTask, WorkFlowInstanceState state)
        {
            var instance = await workFlowInstanceRepository.FindAsync(t => t.Id == endStepTask.InstanceId
                                                                        && t.FlowId == endStepTask.FlowId
                                                                        && t.State == (int)WorkFlowInstanceState.Approving
                                                                        && t.IsDisabled == false);

            if (instance != null)
            {
                instance.State = (int)state;

                await workFlowInstanceRepository.UpdateAsync(instance);

                //如果有临时任务，直接删除掉
                await RemoveTemporaryTask(endStepTask);

                //执行子流程完成事件
                await ExecuteSubFlowFinishedEvent(endStepTask, instance);

                //发送消息
                SendMessage(instance.Title, $"<b>{instance.Title}</b>审批结束！", instance.CreaterId);
            }
        }

        /// <summary>
        /// 成功结束流程实例
        /// </summary>
        /// <returns></returns>
        private async Task CompleteWorkFlowInstance(WorkFlowTask endStepTask)
        {
            await FinishWorkFlowInstance(endStepTask, WorkFlowInstanceState.Completed);
        }

        /// <summary>
        /// 意外关闭流程实例
        /// </summary>
        /// <returns></returns>
        private async Task CloseWorkFlowInstance(WorkFlowTask endStepTask)
        {
            await FinishWorkFlowInstance(endStepTask, WorkFlowInstanceState.Closed);
        }

        /// <summary>
        /// 更新流程进度信息
        /// </summary>
        /// <param name="instanceId"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        private async Task FreshenWorkFlowInstance(Guid instanceId, WorkFlowTask task)
        {
            await workFlowInstanceRepository.UpdateAsync(t => t.Id == instanceId, query => new WorkFlowInstance
            {
                LastExecuteTaskId = task.Id,
                LastExecuteStepId = task.StepId,
                LastExecuteStepName = task.StepName,
                LastModifyTime = DateTime.Now
            });
        }

        #endregion

        #region 处理辅助 

        /// <summary>
        /// 获取当前需要处理的任务
        /// </summary>
        /// <param name="execute"></param>
        /// <returns></returns>
        private async Task<Tuple<WorkFlowTask, WorkFlowStep>> GetTaskAndStepForExecuting(WorkFlowExecute execute)
        {
            WorkFlowTask currentTask = await workFlowTaskRepository.FindAsync(t => t.Id == execute.TaskId);

            if (currentTask == null)
            {
                throw new InvalidOperationException("未能创建或找到当前任务");
            }

            if (currentTask.IsExecute())
            {
                throw new InvalidOperationException("当前任务已处理");
            }

            var currentStep = execute.WorkFlowInstalled.GetStep(execute.StepId);

            if (currentStep == null)
            {
                throw new InvalidOperationException("未找到当前步骤");
            }

            return new Tuple<WorkFlowTask, WorkFlowStep>(currentTask, currentStep);
        }

        /// <summary>
        /// 获取步骤处理人
        /// </summary>
        /// <param name="execute"></param>
        /// <returns></returns>
        private async Task GetExecuteHandler(WorkFlowExecute execute)
        {
            var stepBuild = new Dictionary<Guid, List<WorkFlowUser>>();
            foreach (var item in execute.Steps)
            {
                List<WorkFlowUser> appointHandlers = null;

                //如果有特定人员或者加签人员，则加入
                if (item.Value != null && item.Value.Count > 0)
                {
                    appointHandlers = await GetUser(execute.CompanyId, item.Value.Select(t => t.Id).ToArray());
                }

                var step = execute.WorkFlowInstalled.GetStep(item.Key);

                List<WorkFlowUser> handlers = await GetStepHandler(step, execute, appointHandlers);

                //如果步骤没有处理人，则忽略
                if (handlers.Count > 0)
                {
                    stepBuild.Add(item.Key, handlers);
                }
            }
            execute.Steps = stepBuild;
        }

        /// <summary>
        /// 获取步骤处理人
        /// </summary>
        /// <param name="step"></param>
        /// <param name="execute"></param>
        /// <param name="appointHandlers"></param>
        /// <returns></returns>
        private async Task<List<WorkFlowUser>> GetStepHandler(WorkFlowStep step, WorkFlowExecute execute, List<WorkFlowUser> appointHandlers = null)
        {
            List<WorkFlowUser> handlers = new List<WorkFlowUser>();

            var handlerArray = string.IsNullOrEmpty(step.Handlers) ? null : step.Handlers.Split(',').Select(t => Guid.Parse(t)).ToArray();

            //如果处理人类型是 任意人员 ，则只选择 指定人员
            //如果不是，则根据规则 找到人员 ，将 指定人员 加签

            switch ((WorkFlowHandlerKinds)step.HandlerType)
            {
                case WorkFlowHandlerKinds.AnyUser:

                    //if (appointHandlers != null)
                    //{
                    //    handlers.AddRange(appointHandlers);
                    //}

                    break;
                case WorkFlowHandlerKinds.User:

                    handlers.AddRange(await GetUser(execute.CompanyId, handlerArray));

                    break;
                case WorkFlowHandlerKinds.Department:

                    handlerArray = (await userRepository.FindAllAsync(t => handlerArray.Contains(t.DepartmentId) && t.IsDisabled == false && t.CompanyId == execute.CompanyId)).Select(t => t.Id).ToArray();
                    handlers.AddRange(await GetUser(execute.CompanyId, handlerArray));

                    break;
                case WorkFlowHandlerKinds.UserGroup:

                    handlerArray = (await userGroupMemberRepository.FindAllAsync(t => handlerArray.Contains(t.GroupId))).Select(t => t.UserId).ToArray();
                    handlers.AddRange(await GetUser(execute.CompanyId, handlerArray));

                    break;
                case WorkFlowHandlerKinds.WorkGroup:
                    break;
                case WorkFlowHandlerKinds.Initiator:

                    handlerArray = (await workFlowInstanceRepository.FindAllAsync(t => t.Id == execute.InstanceId)).Select(t => t.CreaterId).ToArray();
                    handlers.AddRange(await GetUser(execute.CompanyId, handlerArray));

                    break;
                case WorkFlowHandlerKinds.DirectorForInitiator:
                    break;
                case WorkFlowHandlerKinds.LeaderForInitiator:
                    break;
                case WorkFlowHandlerKinds.DirectorForHandler:
                    break;
                case WorkFlowHandlerKinds.LeaderForHandler:
                    break;
            }

            if (appointHandlers != null && appointHandlers.Count > 0)
            {
                handlers.AddRange(appointHandlers);
            }

            return handlers.Distinct().ToList();
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userArray"></param>
        /// <returns></returns>
        private async Task<List<WorkFlowUser>> GetUser(Guid companyId, Guid[] userArray)
        {
            if (userArray?.Count() <= 0)
            {
                return new List<WorkFlowUser>();
            }

            var list = await userRepository.FindAllAsync(t => userArray.Contains(t.Id) && t.IsDisabled == false && t.CompanyId == companyId);

            return list.Select(t => new WorkFlowUser
            {
                Id = t.Id,
                Name = t.Name,
                CompanyId = t.CompanyId
            }).ToList();
        }

        /// <summary>
        /// 获得抄送人员
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        private async Task<List<WorkFlowUser>> GetCopyToUsers(WorkFlowStep step, WorkFlowExecute execute)
        {
            if (step.CopyToUsers != default)
            {
                var userArray = step.CopyToUsers.Split(',').Select(t => Guid.Parse(t)).ToArray();
                return await GetUser(execute.CompanyId, userArray);
            }
            else
            {
                return new List<WorkFlowUser>();
            }
        }

        /// <summary>
        /// 处理更新任务撞状态，默认是完成状态
        /// </summary>
        /// <param name="task"></param>
        /// <param name="comment"></param>
        /// <param name="isSign"></param>
        /// <returns></returns>
        private async Task UpdateTaskState(WorkFlowTask task, string comment, bool isSign, WorkFlowTaskState state, string note = "")
        {
            task.Comment = comment;
            task.IsNeedSign = isSign;
            task.State = (int)state;
            task.Note = note;
            task.ExecutedTime = DateTime.Now;
            await workFlowTaskRepository.UpdateAsync(task);
        }

        #endregion

        #region 处理方式

        /// <summary>
        /// 提交任务
        /// </summary>
        /// <param name="execute"></param>
        /// <returns></returns>
        private async Task ExecuteSubmit(WorkFlowExecute execute, WorkFlowInstance instance, Tuple<WorkFlowTask, WorkFlowStep> current)
        {
            var currentTask = current.Item1;
            var currentStep = current.Item2;

            //如果当前步骤是子流程步骤，并且策略是【子流程完成后才能提交】,则要判断子流程是否已完成
            if (!await IsSubFlowStepPassed(currentTask, currentStep, execute))
            {
                throw new InvalidOperationException("当前步骤的子流程未完成，不能提交");
            }

            //判断控制规则，验证步骤
            ExecuteWorkFlowControl(execute, instance, current);

            //获取步骤处理人,在步骤中加入处理人
            await GetExecuteHandler(execute);

            //处理策略判断，返回下一步是否需要等待
            bool isNextStepTaskWating = await ExecuteSubmitHandleTactic(currentTask, currentStep, execute);

            //如果存在下一步骤，如果不存在下一步，则盘算是否是最后一步
            if (execute.Steps.Count > 0)
            {
                if (isNextStepTaskWating)
                {
                    //如果需要等待，说明多人步骤中有人存在待处理任务，则为当前用户创建一个状态为等待中的后续任务，等条件满足后才修改状态，待办人员看不到。
                    await CreateTemporaryTask(currentTask, execute);
                }
                else
                {
                    //创建下一步任务，
                    //如果存在会签任务，则需要判断会签，如果会签没有通过，则还需要继续等待
                    //如果下一步任务创建成功，则之前创建的临时任务将执行激活
                    var nextStepTasks = await CreateNextStepTask(currentTask, currentStep, execute);

                    //下一步任务数大于 0，说明任务已经进行到下一步，则创建抄送任务
                    if (nextStepTasks.Count > 0)
                    {
                        //发送消息
                        SendMessage(instance.Title, $"您有一个新的审批需要处理，关于<b>{instance.Title}</b>.", nextStepTasks.Select(t => t.ReceiverId).Distinct().ToArray());

                        await CreateDuplicateTask(currentTask, execute);
                    }
                    else
                    {
                        //下一步任务没有创建，说明还在等待会签中，则创建下一步的临时任务
                        await CreateTemporaryTask(currentTask, execute);
                    }
                }
            }
            else
            {
                //如果是最后一步，则检查当前步骤是否全部完成，如果完成，将结束流程
                if (currentStep.IsEnd())
                {
                    //如果结束步骤完成，则更新实例状态为已完成
                    var isComplated = await IsStepPassed(currentStep, execute.InstanceId, execute.FlowId, execute.GroupId, currentTask.Sort);

                    if (isComplated)
                    {
                        //结束流程
                        await CompleteWorkFlowInstance(currentTask);
                    }
                }
            }

            // 提交之前所有数据库操作
            await this.repositoryContext.CommitAsync();

            //发送信息
            SendMessage(instance.Title, $"<b>{execute.Sender.Name}</b>处理了<b>{execute.Title}</b>的<b>{currentStep.StepName}</b>.", instance.CreaterId);
        }

        /// <summary>
        /// 退回任务
        /// </summary>
        /// <param name="execute"></param>
        private async Task ExecuteBack(WorkFlowExecute execute, WorkFlowInstance instance, Tuple<WorkFlowTask, WorkFlowStep> current)
        {
            var currentTask = current.Item1;
            var currentStep = current.Item2;

            if (currentStep.IsBackTacticBy(WorkFlowBackTacticKinds.UnableToReturn))
            {
                //不能退回
                throw new InvalidOperationException("当前步骤不能退回");
            }


            //回退策略判断，返回下一步是否需要等待
            bool isNextStepTaskWating = await ExecuteBackBackTactic(currentTask, currentStep, execute);

            if (execute.Steps.Count > 0)
            {
                if (!isNextStepTaskWating)
                {
                    //创建回退任务
                    var backStepTasks = await CreateBackStepTask(currentTask, currentStep, execute);

                    if (backStepTasks.Count > 0)
                    {
                        //发送消息
                        SendMessage(instance.Title, $"<b>{instance.Title}</b>的<b>{currentStep.StepName}</b>被退回，需要您重新处理.", backStepTasks.Select(t => t.ReceiverId).Distinct().ToArray());
                    }
                }
            }
            else
            {
                //如果是第一步，则检查当前步骤判断策略是退回，将结束流程
                if (currentStep.IsStart())
                {
                    //如果结束步骤完成，则更新实例状态为已完成
                    var isBacked = await IsStepBacked(currentStep, execute.InstanceId, execute.FlowId, execute.GroupId, currentTask.Sort);

                    if (isBacked)
                    {
                        //结束流程,属于创建人自己退回，就关闭掉
                        await CloseWorkFlowInstance(currentTask);
                    }
                }
            }

            // 提交之前所有数据库操作
            await this.repositoryContext.CommitAsync();

            //发送信息
            SendMessage(instance.Title, $"<b>{execute.Sender.Name}</b>退回了<b>{execute.Title}</b>的<b>{currentStep.StepName}</b>.", instance.CreaterId);

        }

        /// <summary>
        /// 转交任务
        /// </summary>
        /// <param name="executeModel"></param>
        private async Task ExecuteRedirect(WorkFlowExecute execute, WorkFlowInstance instance, Tuple<WorkFlowTask, WorkFlowStep> current)
        {
            var currentTask = current.Item1;
            var currentStep = current.Item2;

            if (currentTask.Is(WorkFlowTaskState.Waiting))
            {
                throw new InvalidOperationException("当前任务正在等待他人处理");
            }
            if (execute.Steps.First().Value.Count == 0)
            {
                throw new InvalidOperationException("未设置转交人员");
            }

            //获取步骤处理人
            var handlers = await GetUser(execute.CompanyId, execute.Steps.First().Value.Select(t => t.Id).ToArray());
            execute.Steps.First().Value.Clear();
            execute.Steps.First().Value.AddRange(handlers);


            var redirectTasks = new List<WorkFlowTask>();

            foreach (var user in execute.Steps.First().Value)
            {
                WorkFlowTask task = new WorkFlowTask
                {
                    PlannedTime = currentTask.PlannedTime,
                    FlowId = currentTask.FlowId,
                    FlowName = currentTask.FlowName,
                    GroupId = currentTask.GroupId,
                    InstanceId = currentTask.InstanceId,
                    PrevTaskId = currentTask.PrevTaskId,
                    PrevStepId = currentTask.PrevStepId,
                    ReceiveTime = currentTask.ReceiveTime,
                    SenderId = currentTask.SenderId,
                    SenderName = currentTask.SenderName,
                    SendTime = currentTask.SendTime,
                    StepId = currentTask.StepId,
                    StepName = currentTask.StepName,
                    Sort = currentTask.Sort,
                    Title = currentTask.Title,
                    CompanyId = currentTask.CompanyId,

                    ReceiverId = user.Id,
                    ReceiverName = user.Name,
                    State = (int)WorkFlowTaskState.Pending,
                    Type = (int)WorkFlowTaskKinds.Redirect,
                    Note = $"该任务由{currentTask.ReceiverName}转交"
                };

                if (!await IsHasNotExecuteTask(currentTask.FlowId, execute.InstanceId, currentTask.StepId, currentTask.GroupId, user.Id))
                {
                    await workFlowTaskRepository.AddAsync(task);
                }

                redirectTasks.Add(task);
            }

            await UpdateTaskState(currentTask, execute.Comment, execute.IsNeedSign, WorkFlowTaskState.Handled, "已转交他人处理");

            // 提交之前所有数据库操作
            await this.repositoryContext.CommitAsync();

            if (redirectTasks.Count > 0)
            {
                //发送消息
                SendMessage(instance.Title, $"<b>{execute.Sender.Name}</b>将<b>{currentStep.StepName}</b>转交给您审批.", redirectTasks.Select(t => t.ReceiverId).Distinct().ToArray());

                string receiveNames = string.Join(",", redirectTasks.Select(t => t.ReceiverName).Distinct().ToArray());

                //发送信息
                SendMessage(instance.Title, $"<b>{execute.Sender.Name}</b>将<b>{currentStep.StepName}</b>转交给<b>{receiveNames}</b>处理.", instance.CreaterId);
            }

        }

        #endregion

        #region 提交任务相关规则判断

        /// <summary>
        /// 判断处理步骤的控制规则，选择性加入步骤
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="current"></param>
        private void ExecuteWorkFlowControl(WorkFlowExecute execute, WorkFlowInstance instance, Tuple<WorkFlowTask, WorkFlowStep> current)
        {
            var currentStep = current.Item2;
            var currentNextSteps = execute.WorkFlowInstalled.GetNextSteps(current.Item2.StepId);

            //是否下一步有选择任意人员
            if (currentNextSteps.Any(t => t.IsHandleBy(WorkFlowHandlerKinds.AnyUser)))
            {
                //如果没选择步骤，提示
                if (execute.Steps.Count() == 0)
                {
                    throw new InvalidOperationException("未选择下一步步骤");
                }

                foreach (var step in execute.Steps)
                {
                    var s = execute.WorkFlowInstalled.GetStep(step.Key);
                    if (s.IsHandleBy(WorkFlowHandlerKinds.AnyUser))
                    {
                        if (step.Value == null || step.Value.Where(t => !t.Id.Equals(default)).Count() == 0)
                        {
                            throw new InvalidOperationException("有步骤没有选择处理人员");
                        }
                    }
                }
            }

            //判断流程控制
            switch ((WorkFlowControlKinds)current.Item2.FlowControl)
            {
                case WorkFlowControlKinds.System:

                    foreach (var currentNextStep in currentNextSteps)
                    {
                        if (!execute.Steps.ContainsKey(currentNextStep.StepId))
                        {
                            execute.Steps.Add(currentNextStep.StepId, new List<WorkFlowUser>());
                        }
                    }

                    //判断条件通过情况
                    ExecuteConditionsBetweenSteps(execute, instance, current);

                    break;
                case WorkFlowControlKinds.SingleSelect:

                    if (execute.Steps.Count() != 1)
                    {
                        throw new InvalidOperationException("当前步骤必须只能选择一个步骤提交");
                    }

                    break;
                case WorkFlowControlKinds.MultiSelect:

                    //处理步骤如果不是最后一步的提交任务，则检查处理步骤的下一步是否存在
                    if (!(execute.ExecuteType == WorkFlowExecuteKinds.Submit && execute.StepId == WorkFlowConstant.EndStepId) && (execute.Steps == null || execute.Steps.Count == 0))
                    {
                        throw new InvalidOperationException("请选择步骤后提交");
                    }

                    break;
            }
        }

        /// <summary>
        /// 由系统控制时，检测步骤间的条件是否满足
        /// </summary>
        private void ExecuteConditionsBetweenSteps(WorkFlowExecute execute, WorkFlowInstance instance, Tuple<WorkFlowTask, WorkFlowStep> current)
        {
            var formStep = current.Item2;
            var steps = new Dictionary<Guid, List<WorkFlowUser>>();

            //获取数据
            JObject data = JObject.Parse(instance.FormDataJson);

            foreach (var toStep in execute.Steps)
            {
                var transit = execute.WorkFlowInstalled.GetTransit(formStep.StepId, toStep.Key);
                if (transit != null)
                {
                    if (transit.ConditionIs(WorkFlowTransitConditionKinds.None))
                    {
                        steps.Add(toStep.Key, toStep.Value);
                    }
                    else
                    {
                        var condition = transit.ConditionValue.Trim();

                        if (String.IsNullOrEmpty(condition))
                        {
                            steps.Add(toStep.Key, toStep.Value);
                        }
                        else
                        {
                            switch ((WorkFlowTransitConditionKinds)transit.ConditionType)
                            {
                                case WorkFlowTransitConditionKinds.Data:

                                    if (IsDataConditionPassed(data, condition))
                                    {
                                        steps.Add(toStep.Key, toStep.Value);
                                    }

                                    break;
                                case WorkFlowTransitConditionKinds.Method:

                                    if (IsMethodConditionPassed(data, condition))
                                    {
                                        steps.Add(toStep.Key, toStep.Value);
                                    }

                                    break;
                                case WorkFlowTransitConditionKinds.SQL:

                                    if (IsSQLConditionPassed(data, condition))
                                    {
                                        steps.Add(toStep.Key, toStep.Value);
                                    }

                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }

            execute.Steps = steps;
        }

        /// <summary>
        /// 处理提交任务时的处理策略
        /// </summary>
        /// <returns>下一步任务是否需要等待状态</returns>
        private async Task<bool> ExecuteSubmitHandleTactic(WorkFlowTask currentTask, WorkFlowStep currentStep, WorkFlowExecute execute)
        {
            bool isNeedWating = false;

            //第一步和最后一步只有发起者处理，不判断策略
            if (currentTask.IsStart() || currentTask.IsEnd())
            {
                await UpdateTaskState(currentTask, execute.Comment, execute.IsNeedSign, WorkFlowTaskState.Handled);
            }
            else
            {
                //获取当前步骤分发的任务
                var taskList = await GetDistributionTask(currentStep.StepId, currentTask.FlowId, currentTask.InstanceId, currentTask.GroupId, currentTask.Sort);

                switch ((WorkFlowHandleTacticKinds)currentStep.HandleTactic)
                {
                    case WorkFlowHandleTacticKinds.AllAgree://所有人必须处理
                        if (taskList.Count > 1)
                        {
                            var noCompleted = taskList.Where(t => t.IsNot(WorkFlowTaskState.Handled));
                            if (noCompleted.Count() - 1 > 0)
                            {
                                isNeedWating = true;
                            }
                        }
                        await UpdateTaskState(currentTask, execute.Comment, execute.IsNeedSign, WorkFlowTaskState.Handled, isNeedWating ? $"步骤已处理，但还需等待其他人处理" : "");
                        break;
                    case WorkFlowHandleTacticKinds.OneAgree://一人同意即可
                        foreach (var task in taskList)
                        {
                            if (task.Id != currentTask.Id)
                            {
                                if (task.IsNotExecute())
                                {
                                    await UpdateTaskState(task, "", false, WorkFlowTaskState.HandledByOthers, $"步骤有一人同意即可抵达至下一步，{execute.Sender.Name}已选择同意");
                                }
                            }
                            else
                            {
                                await UpdateTaskState(task, execute.Comment, execute.IsNeedSign, WorkFlowTaskState.Handled);
                            }
                        }
                        break;
                    case WorkFlowHandleTacticKinds.PercentageAgree://依据人数比例
                        if (taskList.Count > 1)
                        {
                            decimal percentage = currentStep.Percentage <= 0 ? 100 : currentStep.Percentage;//比例
                            decimal nextPercentage = Math.Round((((decimal)(taskList.Where(t => t.Is(WorkFlowTaskState.Handled)).Count() + 1) / (decimal)taskList.Count) * 100));
                            if (nextPercentage < percentage)
                            {
                                isNeedWating = true;
                            }
                            else
                            {
                                foreach (var task in taskList)
                                {
                                    if (task.Id != currentTask.Id && task.IsNotExecute())
                                    {
                                        await UpdateTaskState(task, "", false, WorkFlowTaskState.HandledByOthers, $"有{nextPercentage}%的人选择同意当前步骤，高于设定比例{percentage}%，由系统判定同意当前步骤");
                                    }
                                }
                            }
                        }
                        await UpdateTaskState(currentTask, execute.Comment, execute.IsNeedSign, WorkFlowTaskState.Handled, isNeedWating ? "步骤已处理，但还需等待其他人处理" : "");
                        break;
                    case WorkFlowHandleTacticKinds.Independent://独立处理
                        await UpdateTaskState(currentTask, execute.Comment, execute.IsNeedSign, WorkFlowTaskState.Handled);
                        break;
                }
            }

            return isNeedWating;
        }

        /// <summary>
        /// 创建下一步任务，如果任务存在等待状态将执行激活
        /// </summary>
        /// <returns></returns>
        private async Task<List<WorkFlowTask>> CreateNextStepTask(WorkFlowTask currentTask, WorkFlowStep currentStep, WorkFlowExecute execute)
        {
            var nextStepTasks = new List<WorkFlowTask>();

            foreach (var step in execute.Steps)
            {
                foreach (var user in step.Value)
                {
                    // 步骤如果不存在，直接忽略
                    var nextStep = execute.WorkFlowInstalled.GetStep(step.Key);
                    if (nextStep == null)
                    {
                        continue;
                    }

                    bool isPass = nextStep.IsNotNeedCountersignature();

                    // 如果下一步骤需要会签，则要检查当前步骤的平级步骤是否已处理
                    if (nextStep.IsNeedCountersignature())
                    {
                        isPass = await IsCountersignatureStepPassed(currentTask, nextStep, execute);

                        if (isPass)
                        {
                            // 会签通过，处理会签节点之前的所有需要会签节点任务
                            var sameLevelStepTaskList = await GetSameLevelStepNotExecuteTask(currentTask, execute);
                            foreach (var task in sameLevelStepTaskList)
                            {
                                if (task.Id == currentTask.Id || task.IsExecute())
                                {
                                    continue;
                                }
                                await UpdateTaskState(task, "", false, WorkFlowTaskState.HandledByOthers, "会签已结束，此步骤因其他人员完成通过");
                            }
                        }
                    }

                    if (isPass)
                    {
                        WorkFlowTask task = new WorkFlowTask();
                        if (nextStep.TimeLimit > 0)
                        {
                            task.PlannedTime = DateTime.Now.AddHours((double)nextStep.TimeLimit);
                        }

                        task.FlowId = execute.FlowId;
                        task.FlowName = currentTask.FlowName;
                        task.GroupId = currentTask != null ? currentTask.GroupId : execute.GroupId;
                        task.Type = (int)WorkFlowTaskKinds.Normal;
                        task.InstanceId = execute.InstanceId;
                        task.Note = execute.Note;
                        task.PrevTaskId = currentTask.Id;
                        task.PrevStepId = currentTask.StepId;
                        task.ReceiverId = user.Id;
                        task.ReceiverName = user.Name;
                        task.ReceiveTime = DateTime.Now;
                        task.SenderId = execute.Sender.Id;
                        task.SenderName = execute.Sender.Name;
                        task.SendTime = task.ReceiveTime;
                        task.State = (int)WorkFlowTaskState.Pending;
                        task.StepId = nextStep.StepId;
                        task.StepName = nextStep.StepName;
                        task.Sort = currentTask.Sort + 1;
                        task.Title = string.IsNullOrEmpty(execute.Title) ? currentTask.Title : execute.Title;
                        task.CompanyId = execute.CompanyId;

                        // 如果当前步骤是子流程步骤，则要发起子流程实例
                        if (nextStep.IsSubFlowStep() && !string.IsNullOrEmpty(nextStep.SubFlowId))
                        {
                            var subflowTask = await CreateSubFlowStepTask(task, nextStep, user);
                            task.SubFlowInstanceId = subflowTask.InstanceId;
                        }

                        // 检查当前用户在此步骤是否已被创建任务
                        if (!await IsHasNotExecuteTask(execute.FlowId, execute.InstanceId, step.Key, currentTask.GroupId, user.Id))
                        {
                            await workFlowTaskRepository.AddAsync(task);
                        }

                        nextStepTasks.Add(task);
                    }
                }
            }

            if (nextStepTasks.Count > 0)
            {
                //激活临时任务
                var stepArray = nextStepTasks.Select(t => t.StepId).Distinct().ToArray();
                var waitingTasks = await workFlowTaskRepository.FindAllAsync(t => t.FlowId == nextStepTasks[0].FlowId
                                                                                           && t.InstanceId == nextStepTasks[0].InstanceId
                                                                                           && stepArray.Contains(t.StepId)
                                                                                           && t.GroupId == nextStepTasks[0].GroupId
                                                                                           && t.State == (int)WorkFlowTaskState.Waiting
                                                                                           && t.IsDisabled == false);

                if (waitingTasks.Count() > 0)
                {
                    foreach (var waitingTask in waitingTasks)
                    {
                        waitingTask.PlannedTime = nextStepTasks[0].PlannedTime;
                        waitingTask.ReceiveTime = nextStepTasks[0].ReceiveTime;
                        waitingTask.SendTime = nextStepTasks[0].ReceiveTime;
                        waitingTask.State = (int)WorkFlowTaskState.Pending;
                        await workFlowTaskRepository.UpdateAsync(waitingTask);
                    }
                }
            }

            return nextStepTasks;
        }

        #endregion

        #region 回退任务相关规则判断

        /// <summary>
        /// 执行退回策略
        /// </summary>
        /// <param name="currentTask"></param>
        /// <param name="currentStep"></param>
        /// <param name="execute"></param>
        /// <returns></returns>
        private async Task<bool> ExecuteBackBackTactic(WorkFlowTask currentTask, WorkFlowStep currentStep, WorkFlowExecute execute)
        {
            bool isNeedWating = false;

            //第一步退回，不判断策略
            if (currentTask.IsStart())
            {
                await UpdateTaskState(currentTask, execute.Comment, execute.IsNeedSign, WorkFlowTaskState.Returned);
            }
            else
            {
                //获取当前步骤分发的所有任务
                var taskList = await GetDistributionTask(currentStep.StepId, currentTask.FlowId, currentTask.InstanceId, currentTask.GroupId, currentTask.Sort);

                if (currentStep.IsBackTacticBy(WorkFlowBackTacticKinds.ReturnByTactic))
                {
                    //依据策略退回 
                    switch ((WorkFlowHandleTacticKinds)currentStep.HandleTactic)
                    {
                        case WorkFlowHandleTacticKinds.AllAgree: //所有人必须同意,如果一人不同意则全部退回
                            foreach (var task in taskList)
                            {
                                if (task.Id != currentTask.Id)
                                {
                                    if (task.IsNotExecute())
                                    {
                                        await UpdateTaskState(task, "", false, WorkFlowTaskState.ReturnedByOthers, $"{execute.Sender.Name}已选择退回，步骤必须全员同意才可以抵达至下一步");
                                    }
                                }
                                else
                                {
                                    await UpdateTaskState(task, execute.Comment, execute.IsNeedSign, WorkFlowTaskState.Returned);
                                }
                            }
                            break;
                        case WorkFlowHandleTacticKinds.OneAgree://一人同意即可，退回自己的，不影响其他人
                            if (taskList.Count > 1)
                            {
                                var noReturned = taskList.Where(t => t.IsNot(WorkFlowTaskState.Returned));
                                if (noReturned.Count() - 1 > 0)
                                {
                                    isNeedWating = true;
                                }
                            }
                            await UpdateTaskState(currentTask, execute.Comment, execute.IsNeedSign, WorkFlowTaskState.Returned, isNeedWating ? "步骤已退回，但还需等待其他人处理" : "");
                            break;
                        case WorkFlowHandleTacticKinds.PercentageAgree://依据人数比例，投票比例大于规定比例,则退回全部，如果小于则等待，其他人继续投票
                            if (taskList.Count > 1)
                            {
                                decimal percentage = 100 - (currentStep.Percentage <= 0 ? 100 : currentStep.Percentage);//比例
                                decimal backPercentage = Math.Round(((decimal)(taskList.Where(t => t.Is(WorkFlowTaskState.Returned)).Count() + 1) / (decimal)taskList.Count) * 100);
                                if (backPercentage < percentage)
                                {
                                    isNeedWating = true;
                                }
                                else
                                {
                                    foreach (var task in taskList)
                                    {
                                        if (task.Id != currentTask.Id && task.IsNotExecute())
                                        {
                                            await UpdateTaskState(task, "", false, WorkFlowTaskState.ReturnedByOthers, $"有{backPercentage}%的人选择退回当前步骤，高于设定比例{percentage}%，由系统判定退回当前步骤");
                                        }
                                    }
                                }
                            }
                            await UpdateTaskState(currentTask, execute.Comment, execute.IsNeedSign, WorkFlowTaskState.Returned, isNeedWating ? "步骤已退回，但还需等待其他人处理" : "");
                            break;
                        case WorkFlowHandleTacticKinds.Independent://独立处理
                            await UpdateTaskState(currentTask, execute.Comment, execute.IsNeedSign, WorkFlowTaskState.Returned);
                            break;
                    }
                }
                else
                {
                    //不管三七二十一，直接退回
                    foreach (var task in taskList)
                    {
                        if (task.Is(WorkFlowTaskState.Handled, WorkFlowTaskState.Returned))//已完成的任务不能退回
                        {
                            continue;
                        }
                        if (task.Id == currentTask.Id)
                        {
                            await UpdateTaskState(task, execute.Comment, execute.IsNeedSign, WorkFlowTaskState.Returned);
                        }
                        else
                        {
                            await UpdateTaskState(task, "", false, WorkFlowTaskState.Returned, $"当前步骤已被{execute.Sender.Name}退回");
                        }
                    }
                }

                //退回时要退回其它步骤发来的同级任务
                foreach (var task in taskList)
                {
                    if (!execute.Steps.ContainsKey(task.PrevStepId))
                    {
                        execute.Steps.Add(task.PrevStepId, new List<WorkFlowUser>());
                    }
                }
                //如果当前步骤是会签步骤，则退回重新会签
                if (currentStep.IsNeedCountersignature())
                {
                    var countersignatureStepPrevSteps = execute.WorkFlowInstalled.GetPrevSteps(currentStep.StepId);
                    foreach (var prevStep in countersignatureStepPrevSteps)
                    {
                        if (!execute.Steps.ContainsKey(prevStep.StepId))
                        {
                            execute.Steps.Add(prevStep.StepId, new List<WorkFlowUser>());
                        }
                    }
                }
            }

            return isNeedWating;
        }

        /// <summary>
        /// 创建退回步骤
        /// </summary>
        /// <param name="currentTask"></param>
        /// <param name="currentStep"></param>
        /// <param name="execute"></param>
        /// <returns></returns>
        private async Task<List<WorkFlowTask>> CreateBackStepTask(WorkFlowTask currentTask, WorkFlowStep currentStep, WorkFlowExecute execute)
        {
            var sourceBackStepTasks = new List<WorkFlowTask>();

            foreach (var step in execute.Steps)
            {
                //获取当前步骤分发的最新任务
                var backStepTask = await GetNewestDistributionTask(step.Key, currentTask.FlowId, currentTask.InstanceId, currentTask.GroupId);
                sourceBackStepTasks.AddRange(backStepTask);
            }

            //当前步骤的下一步是否是会签步骤，处理会签形式的退回
            var countersignatureStep = execute.WorkFlowInstalled.GetNextSteps(currentStep.StepId).Find(t => t.IsNeedCountersignature());
            bool isCountersignature = countersignatureStep != null;
            bool isBack = true;
            if (isCountersignature)
            {
                isBack = await IsCountersignatureStepBacked(currentTask, countersignatureStep, execute);

                if (isBack)
                {
                    var sameLevelStepTaskList = await GetSameLevelStepNotExecuteTask(currentTask, execute);
                    foreach (var task in sameLevelStepTaskList)
                    {
                        if (task.Id == currentTask.Id || task.IsExecute())
                        {
                            continue;
                        }
                        await UpdateTaskState(task, "", false, WorkFlowTaskState.ReturnedByOthers, "会签已结束，此步骤因其他人员完成回退");
                    }
                }
            }

            //当前步骤是子流程步骤，则要作废子流程实例
            if (currentStep.IsSubFlowStep() && !string.IsNullOrEmpty(currentStep.SubFlowId))
            {
                await RemoveWorkFlowInstance(currentTask.SubFlowInstanceId, Guid.Parse(currentStep.SubFlowId), currentTask.GroupId);
            }

            var backStepTasks = new List<WorkFlowTask>();
            if (isBack)
            {
                sourceBackStepTasks = sourceBackStepTasks.Distinct((WorkFlowTask x, WorkFlowTask y) => x.ReceiverId == y.ReceiverId && x.StepId == y.StepId && x.FlowId == y.FlowId && x.InstanceId == y.InstanceId).ToList();

                if (sourceBackStepTasks.Count > 0)
                {
                    foreach (var backStepTask in sourceBackStepTasks)
                    {
                        WorkFlowTask task = new WorkFlowTask();
                        if (currentStep.TimeLimit > 0)
                        {
                            task.PlannedTime = DateTime.Now.AddHours((double)currentStep.TimeLimit);
                        }
                        task.FlowId = backStepTask.FlowId;
                        task.FlowName = backStepTask.FlowName;
                        task.GroupId = backStepTask.GroupId;
                        task.Type = (int)WorkFlowTaskKinds.Return;
                        task.InstanceId = backStepTask.InstanceId;
                        task.Note = $"由步骤【{currentTask.StepName}】执行退回所产生的任务";
                        task.PrevTaskId = currentTask.Id;
                        task.PrevStepId = backStepTask.PrevStepId;
                        task.ReceiverId = backStepTask.ReceiverId;
                        task.ReceiverName = backStepTask.ReceiverName;
                        task.ReceiveTime = DateTime.Now;
                        task.SenderId = currentTask.ReceiverId;
                        task.SenderName = currentTask.ReceiverName;
                        task.SendTime = DateTime.Now;
                        task.State = (int)WorkFlowTaskState.Pending;
                        task.StepId = backStepTask.StepId;
                        task.StepName = backStepTask.StepName;
                        task.Sort = currentTask.Sort + 1;
                        task.Title = backStepTask.Title;
                        task.CompanyId = backStepTask.CompanyId;

                        await workFlowTaskRepository.AddAsync(task);
                        backStepTasks.Add(task);
                    }

                    //删除临时任务
                    if (isCountersignature)
                    {
                        await RemoveTemporaryTask(currentTask, true);
                    }
                    else
                    {
                        await RemoveTemporaryTask(currentTask, false);
                    }
                }
            }

            return backStepTasks;
        }

        #endregion

        #region 第一个任务

        /// <summary>
        /// 创建第一个任务
        /// </summary>
        /// <returns></returns>
        private async Task<WorkFlowTask> CreateFirstTask(WorkFlowExecute execute)
        {
            var firstStep = execute.WorkFlowInstalled.GetFirstStep();

            var task = new WorkFlowTask();

            task.Title = string.IsNullOrEmpty(execute.Title) ? "未命名任务(" + execute.WorkFlowInstalled.Name + ")" : execute.Title;
            task.FlowId = execute.FlowId;
            task.FlowName = execute.FlowName;
            task.GroupId = execute.GroupId;
            task.Type = (int)WorkFlowTaskKinds.Normal;
            task.InstanceId = execute.InstanceId;
            task.Note = execute.Note;
            task.PrevTaskId = default;
            task.PrevStepId = default;
            task.ReceiverId = execute.Sender.Id;
            task.ReceiverName = execute.Sender.Name;
            task.ReceiveTime = DateTime.Now;
            task.SenderId = execute.Sender.Id;
            task.SenderName = execute.Sender.Name;
            task.SendTime = task.ReceiveTime;
            task.State = (int)WorkFlowTaskState.Pending;
            task.StepId = firstStep.StepId;
            task.StepName = firstStep.StepName;
            task.Sort = 1;
            task.CompanyId = execute.CompanyId;

            if (firstStep.TimeLimit > 0)
            {
                task.PlannedTime = DateTime.Now.AddHours((double)firstStep.TimeLimit);
            }

            await workFlowTaskRepository.AddAsync(task);

            return task;
        }

        #endregion

        #region 子流程任务

        /// <summary>
        /// 如果下一步为子流程步骤，则创建子流程实例
        /// </summary>
        /// <param name="nextStep"></param>
        /// <param name="currentTask"></param>
        /// <param name="execute"></param>
        /// <param name="sender"></param>
        /// <returns></returns>
        private async Task<WorkFlowTask> CreateSubFlowStepTask(WorkFlowTask subflowTask, WorkFlowStep subflowStep, WorkFlowUser sender)
        {
            //创建子实例
            WorkFlowInstance subflowInstance = new WorkFlowInstance
            {
                FormJson = "",
                FormDataJson = "",
                Title = $"由【{subflowTask.Title}】分支的子流程审批",
                FlowId = Guid.Parse(subflowStep.SubFlowId),
                CreaterId = sender.Id,
                CreaterName = sender.Name
            };

            //执行子流程触发前事件，可影响子流程创建
            var result = ExecuteSubFlowActivationBeforeEvent(subflowTask, subflowStep, subflowInstance);

            subflowInstance = result.SubFlowInstance;

            subflowInstance.CompanyId = subflowTask.CompanyId;
            subflowInstance.IsDisabled = false;

            var subflowFirstTask = await StartWorkFlowInstance(subflowInstance, subflowTask.GroupId);

            //执行子流程触发后事件
            ExecuteSubFlowActivationAfterEvent(subflowTask, subflowStep, subflowInstance);

            return subflowFirstTask;
        }

        #endregion

        #region  抄送任务

        /// <summary>
        /// 创建抄送任务
        /// </summary>
        /// <param name="currentTask"></param>
        /// <param name="execute"></param>
        /// <returns></returns>
        private async Task CreateDuplicateTask(WorkFlowTask currentTask, WorkFlowExecute execute)
        {
            foreach (var step in execute.Steps)
            {
                var nextStep = execute.WorkFlowInstalled.GetStep(step.Key);

                if (nextStep == null)
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(nextStep.CopyToUsers))
                {
                    var users = await GetCopyToUsers(nextStep, execute);
                    foreach (var user in users)
                    {
                        WorkFlowTask task = new WorkFlowTask();
                        if (nextStep.TimeLimit > 0)
                        {
                            task.PlannedTime = DateTime.Now.AddHours((double)nextStep.TimeLimit);
                        }
                        task.FlowId = execute.FlowId;
                        task.FlowName = currentTask.FlowName;
                        task.GroupId = currentTask != null ? currentTask.GroupId : execute.GroupId;
                        task.Type = (int)WorkFlowTaskKinds.Copy;
                        task.InstanceId = execute.InstanceId;
                        task.Note = string.IsNullOrEmpty(execute.Note) ? "抄送任务" : execute.Note + "(抄送任务)";
                        task.PrevTaskId = currentTask.Id;
                        task.PrevStepId = currentTask.StepId;
                        task.ReceiverId = user.Id;
                        task.ReceiverName = user.Name;
                        task.ReceiveTime = DateTime.Now;
                        task.SenderId = execute.Sender.Id;
                        task.SenderName = execute.Sender.Name;
                        task.SendTime = task.ReceiveTime;
                        task.State = (int)WorkFlowTaskState.Pending;
                        task.StepId = step.Key;
                        task.StepName = nextStep.StepName;
                        task.Sort = currentTask.Sort + 1;
                        task.Title = string.IsNullOrEmpty(execute.Title) ? currentTask.Title : execute.Title;
                        task.CompanyId = execute.CompanyId;

                        if (!await IsHasNotExecuteTask(execute.FlowId, execute.InstanceId, step.Key, currentTask.GroupId, user.Id))
                        {
                            await workFlowTaskRepository.AddAsync(task);
                        }
                    }
                }
            }
        }

        #endregion

        #region 临时任务

        /// <summary>
        /// 创建临时任务
        /// </summary>
        /// <param name="execute"></param>
        private async Task<List<WorkFlowTask>> CreateTemporaryTask(WorkFlowTask currentTask, WorkFlowExecute execute)
        {
            List<WorkFlowTask> tasks = new List<WorkFlowTask>();
            foreach (var step in execute.Steps)
            {
                foreach (var user in step.Value)
                {
                    var nextStep = execute.WorkFlowInstalled.GetStep(step.Key);
                    if (nextStep == null)
                    {
                        continue;
                    }
                    WorkFlowTask task = new WorkFlowTask();
                    if (nextStep.TimeLimit > 0)
                    {
                        task.PlannedTime = DateTime.Now.AddHours((double)nextStep.TimeLimit);
                    }
                    task.FlowId = execute.FlowId;
                    task.FlowName = currentTask.FlowName;
                    task.GroupId = currentTask != null ? currentTask.GroupId : execute.GroupId;
                    task.Type = (int)WorkFlowTaskKinds.Normal;
                    task.InstanceId = execute.InstanceId;
                    task.Note = execute.Note;
                    task.PrevTaskId = currentTask.Id;
                    task.PrevStepId = currentTask.StepId;
                    task.ReceiverId = user.Id;
                    task.ReceiverName = user.Name;
                    task.ReceiveTime = DateTime.Now;
                    task.SenderId = execute.Sender.Id;
                    task.SenderName = execute.Sender.Name;
                    task.SendTime = task.ReceiveTime;
                    task.State = (int)WorkFlowTaskState.Waiting;
                    task.StepId = step.Key;
                    task.StepName = nextStep.StepName;
                    task.Sort = currentTask.Sort + 1;
                    task.Title = string.IsNullOrEmpty(execute.Title) ? currentTask.Title : execute.Title;
                    task.CompanyId = execute.CompanyId;

                    if (!await IsHasNotExecuteTask(execute.FlowId, execute.InstanceId, step.Key, currentTask.GroupId, user.Id))
                    {
                        await workFlowTaskRepository.AddAsync(task);
                    }
                    tasks.Add(task);
                }
            }
            return tasks;
        }

        /// <summary>
        /// 删除临时任务
        /// </summary>
        /// <param name="currentTask"></param>
        /// <param name="execute"></param>
        /// <returns></returns>
        private async Task RemoveTemporaryTask(WorkFlowTask currentTask, bool isRemoveAll = true)
        {
            IEnumerable<WorkFlowTask> workFlowTasks;

            if (isRemoveAll)
            {
                workFlowTasks = await workFlowTaskRepository.FindAllAsync(t => t.InstanceId == currentTask.InstanceId
                                                                            && t.FlowId == currentTask.FlowId
                                                                            && t.GroupId == currentTask.GroupId
                                                                            && t.State == (int)WorkFlowTaskState.Waiting);


            }
            else
            {
                workFlowTasks = await workFlowTaskRepository.FindAllAsync(t => t.InstanceId == currentTask.InstanceId
                                                                            && t.FlowId == currentTask.FlowId
                                                                            && t.GroupId == currentTask.GroupId
                                                                            && t.PrevStepId == currentTask.StepId
                                                                            && t.State == (int)WorkFlowTaskState.Waiting);

            }

            if (workFlowTasks?.Count() > 0)
            {
                foreach (var task in workFlowTasks)
                {
                    await workFlowTaskRepository.RemoveAsync(task);
                }
            }
        }

        #endregion

        #region 判断步骤是通过还是回退

        /// <summary>
        /// 查询一个用户在一个步骤是否有未处理任务
        /// </summary>
        /// <returns></returns>
        private async Task<bool> IsHasNotExecuteTask(Guid flowId, Guid instanceId, Guid stepId, Guid groupId, Guid userId)
        {
            var count = await workFlowTaskRepository.CountAsync(t => t.FlowId == flowId
                                                                  && t.InstanceId == instanceId
                                                                  && t.StepId == stepId
                                                                  && t.GroupId == groupId
                                                                  && t.ReceiverId == userId
                                                                  && t.IsNotExecute());

            return count > 0;
        }

        /// <summary>
        /// 如果当前步骤是子流程步骤，并且策略是【子流程完成后才能提交】,则要判断子流程是否已完成
        /// </summary>
        /// <returns></returns>
        private async Task<bool> IsSubFlowStepPassed(WorkFlowTask currentTask, WorkFlowStep currentStep, WorkFlowExecute execute)
        {
            bool isPass = true;

            if (currentStep.StepType == (int)WorkFlowStepKinds.SubFlow
                && !string.IsNullOrEmpty(currentStep.SubFlowId)
                && currentTask.SubFlowInstanceId != default)
            {
                if (currentStep.SubFlowTactic != (int)WorkFlowSubFlowTacticKinds.SubFlowStarted)
                {
                    var wrokFlowInstance = await workFlowInstanceRepository.FindAsync(t => t.Id == currentTask.SubFlowInstanceId && t.FlowId == Guid.Parse(currentStep.SubFlowId) && t.IsDisabled == false);

                    if (wrokFlowInstance == null)
                    {
                        throw new InvalidOperationException("当前步骤的子流程未找到，不能提交");
                    }

                    switch ((WorkFlowSubFlowTacticKinds)currentStep.SubFlowTactic)
                    {
                        case WorkFlowSubFlowTacticKinds.SubFlowCompleted:

                            isPass = wrokFlowInstance.State == (int)WorkFlowInstanceState.Completed;

                            break;
                        case WorkFlowSubFlowTacticKinds.SubFlowFinished:

                            isPass = wrokFlowInstance.State != (int)WorkFlowInstanceState.Approving;

                            break;
                    }
                }

                //isPass = (await _workFlowTaskService.GetCountAsync(t => t.FlowId == currentStep.SubFlowId
                //&& t.IsNotExecute()
                //&& t.InstanceId == currentTask.SubFlowInstanceId
                //&& t.GroupId == currentTask.GroupId)) == 0;

                return isPass;
            }

            return isPass;
        }

        /// <summary>
        /// 会签步骤是否可以完成
        /// </summary>
        /// <returns></returns>
        private async Task<bool> IsCountersignatureStepPassed(WorkFlowTask currentTask, WorkFlowStep countersignatureStep, WorkFlowExecute execute)
        {
            //获取会签步骤的所有上一步
            var prevSteps = execute.WorkFlowInstalled.GetPrevSteps(countersignatureStep.StepId);

            var isPass = true;

            switch ((WorkFlowCountersignatureTacticKinds)countersignatureStep.CountersignatureTactic)
            {
                case WorkFlowCountersignatureTacticKinds.AllAgree://所有步骤同意
                    isPass = true;
                    foreach (var prevStep in prevSteps)
                    {
                        if (!await IsStepPassed(prevStep, execute.InstanceId, execute.FlowId, execute.GroupId, currentTask.Sort))
                        {
                            isPass = false;
                            break;
                        }
                    }
                    break;
                case WorkFlowCountersignatureTacticKinds.OneAgree://一个步骤同意即可
                    isPass = false;
                    foreach (var prevStep in prevSteps)
                    {
                        if (await IsStepPassed(prevStep, execute.InstanceId, execute.FlowId, execute.GroupId, currentTask.Sort))
                        {
                            isPass = true;
                            break;
                        }
                    }
                    break;
                case WorkFlowCountersignatureTacticKinds.PercentageAgree://依据比例
                    int passCount = 0;
                    foreach (var prevStep in prevSteps)
                    {
                        if (await IsStepPassed(prevStep, execute.InstanceId, execute.FlowId, execute.GroupId, currentTask.Sort))
                        {
                            passCount++;
                        }
                    }
                    isPass = Math.Round(((decimal)passCount / (decimal)prevSteps.Count) * 100) >= (countersignatureStep.CountersignaturePercentage <= 0 ? 100 : countersignatureStep.CountersignaturePercentage);
                    break;
            }

            return isPass;
        }

        /// <summary>
        /// 判断一个步骤是否已完成
        /// </summary>
        /// <returns></returns>
        private async Task<bool> IsStepPassed(WorkFlowStep step, Guid instanceId, Guid flowId, Guid groupId, int sort)
        {
            var tasks = await GetDistributionTask(step.StepId, flowId, instanceId, groupId, sort);

            if (tasks.Count == 0)
            {
                return false;
            }

            bool isPassing = true;

            switch (step.HandleTactic)
            {
                case 0://所有人必须处理
                case 3://独立处理
                    isPassing = tasks.Where(t => t.IsNot(WorkFlowTaskState.Handled)).Count() == 0;
                    break;
                case 1://一人同意即可
                    isPassing = tasks.Where(t => t.Is(WorkFlowTaskState.Handled)).Count() > 0;
                    break;
                case 2://依据人数比例
                    isPassing = Math.Round(((decimal)(tasks.Where(t => t.Is(WorkFlowTaskState.Handled)).Count() + 1) / (decimal)tasks.Count) * 100) >= (step.Percentage <= 0 ? 100 : step.Percentage);
                    break;
            }

            return isPassing;
        }

        /// <summary>
        /// 检查会签步骤是否可以退回
        /// </summary>
        /// <returns></returns>
        private async Task<bool> IsCountersignatureStepBacked(WorkFlowTask currentTask, WorkFlowStep countersignatureStep, WorkFlowExecute execute)
        {
            //获取会签步骤的所有上一步
            var prevSteps = execute.WorkFlowInstalled.GetPrevSteps(countersignatureStep.StepId);

            var isBack = true;

            switch ((WorkFlowCountersignatureTacticKinds)countersignatureStep.CountersignatureTactic)
            {
                case WorkFlowCountersignatureTacticKinds.AllAgree://所有步骤处理，如果一个步骤退回则退回
                    isBack = false;
                    foreach (var step in prevSteps)
                    {
                        if (await IsStepBacked(step, execute.InstanceId, execute.FlowId, currentTask.GroupId, currentTask.Sort))
                        {
                            isBack = true;
                            break;
                        }
                    }
                    break;
                case WorkFlowCountersignatureTacticKinds.OneAgree://一个步骤退回,如果有一个步骤同意，则不退回
                    foreach (var step in prevSteps)
                    {
                        if (!await IsStepBacked(step, execute.InstanceId, execute.FlowId, currentTask.GroupId, currentTask.Sort))
                        {
                            isBack = false;
                            break;
                        }
                    }
                    break;
                case WorkFlowCountersignatureTacticKinds.PercentageAgree://依据比例退回
                    int backCount = 0;
                    foreach (var step in prevSteps)
                    {
                        if (await IsStepBacked(step, execute.InstanceId, execute.FlowId, currentTask.GroupId, currentTask.Sort))
                        {
                            backCount++;
                        }
                    }
                    isBack = Math.Round(((decimal)backCount / (decimal)prevSteps.Count) * 100) >= (countersignatureStep.CountersignaturePercentage <= 0 ? 100 : countersignatureStep.CountersignaturePercentage);
                    break;
            }

            return isBack;
        }

        /// <summary>
        /// 判断一个步骤是否退回
        /// </summary>
        /// <returns></returns>
        private async Task<bool> IsStepBacked(WorkFlowStep step, Guid instanceId, Guid flowId, Guid groupId, int sort)
        {
            var tasks = await GetDistributionTask(step.StepId, flowId, instanceId, groupId, sort);
            if (tasks.Count == 0)
            {
                return false;
            }
            bool isBack = true;

            //如果是依据策略退回的，判断策略，如果不是，则有一人退回，即退回
            if (step.IsBackTacticBy(WorkFlowBackTacticKinds.ReturnByTactic))
            {
                switch ((WorkFlowHandleTacticKinds)step.HandleTactic)
                {
                    case WorkFlowHandleTacticKinds.AllAgree://所有人必须处理，一人退回，即可退回
                    case WorkFlowHandleTacticKinds.Independent://独立处理
                        isBack = tasks.Where(t => t.Is(WorkFlowTaskState.Returned, WorkFlowTaskState.ReturnedByOthers)).Count() > 0;
                        break;
                    case WorkFlowHandleTacticKinds.OneAgree://一人同意即可，没有人处理通过，即可退回
                        isBack = tasks.Where(t => t.Is(WorkFlowTaskState.Handled, WorkFlowTaskState.HandledByOthers)).Count() == 0;
                        break;
                    case WorkFlowHandleTacticKinds.PercentageAgree://依据人数比例
                        isBack = Math.Round(((decimal)(tasks.Where(t => t.Is(WorkFlowTaskState.Returned, WorkFlowTaskState.ReturnedByOthers)).Count() + 1) / (decimal)tasks.Count) * 100) >= 100 - (step.Percentage <= 0 ? 100 : step.Percentage);
                        break;
                }
            }
            else
            {
                isBack = tasks.Where(t => t.Is(WorkFlowTaskState.Returned, WorkFlowTaskState.ReturnedByOthers)).Count() > 0;
            }
            return isBack;
        }

        #endregion

        #region 自定义事件

        /// <summary>
        /// 执行自定义方法
        /// </summary>
        /// <returns></returns>
        private TResult ExecuteCustomMethod<TArgs, TResult>(string name, TArgs args)
        {
            var reflection = name.Split(',');
            var dllName = reflection[0];
            var typeName = System.IO.Path.GetFileNameWithoutExtension(reflection[1]);
            var methodName = System.IO.Path.GetExtension(reflection[1]).Substring(1);

            var assembly = Assembly.Load(dllName);
            Type type = assembly.GetType(typeName, true);
            var instance = System.Activator.CreateInstance(type, false);
            var method = type.GetMethod(methodName);

            if (method != null)
            {
                var result = method.Invoke(instance, new object[] { args });

                if (result is TResult)
                {
                    return (TResult)result;
                }
                else
                {
                    throw new TypeUnloadedException($"{name} 提供的返回值类型不符合规定");
                }
            }
            else
            {
                throw new MissingMethodException(typeName, methodName);
            }
        }

        /// <summary>
        /// 激活子流程前事件
        /// </summary>
        /// <returns></returns>
        private SubFlowActivationBeforeEventResults ExecuteSubFlowActivationBeforeEvent(WorkFlowTask subflowTask, WorkFlowStep subflowStep, WorkFlowInstance subflowInstance)
        {
            if (!string.IsNullOrEmpty(subflowStep.SubFlowActivationBeforeEvent))
            {
                var eventName = subflowStep.SubFlowActivationBeforeEvent.Trim();
                var eventArgs = new SubFlowActivationBeforeEventArgs()
                {
                    FlowId = subflowTask.FlowId,
                    GroupId = subflowTask.GroupId,
                    InstanceId = subflowTask.InstanceId,
                    StepId = subflowTask.StepId,
                    TaskId = subflowTask.Id,
                    SubFlowInstance = subflowInstance
                };
                var result = ExecuteCustomMethod<SubFlowActivationBeforeEventArgs, SubFlowActivationBeforeEventResults>(eventName, eventArgs);

                if (result.IsError)
                {
                    throw new InvalidOperationException(result.ErrorMessage);
                }

                return result;
            }

            return null;
        }

        /// <summary>
        /// 激活子流程后事件
        /// </summary>
        /// <returns></returns>
        private SubFlowActivationAfterEventResults ExecuteSubFlowActivationAfterEvent(WorkFlowTask subflowTask, WorkFlowStep subflowStep, WorkFlowInstance subflowInstance)
        {
            if (!string.IsNullOrEmpty(subflowStep.SubFlowActivationAfterEvent))
            {
                var eventName = subflowStep.SubFlowActivationAfterEvent.Trim();
                var eventArgs = new SubFlowActivationAfterEventArgs()
                {
                    FlowId = subflowTask.FlowId,
                    GroupId = subflowTask.GroupId,
                    InstanceId = subflowTask.InstanceId,
                    StepId = subflowTask.StepId,
                    TaskId = subflowTask.Id,
                    SubFlowInstance = subflowInstance
                };
                var result = ExecuteCustomMethod<SubFlowActivationAfterEventArgs, SubFlowActivationAfterEventResults>(eventName, eventArgs);

                if (result.IsError)
                {
                    throw new InvalidOperationException(result.ErrorMessage);
                }

                return result;
            }

            return null;
        }

        /// <summary>
        /// 执行子流程完成后事件
        /// </summary>
        /// <returns></returns>
        private async Task<SubFlowFinishedEventResults> ExecuteSubFlowFinishedEvent(WorkFlowTask subflowTask, WorkFlowInstance subflowInstance)
        {
            var parentTask = await GetParentTask(subflowTask);
            // 执行子流程完成后事件
            if (parentTask != null)
            {
                var subflowInstanceEntity = await workFlowInstanceRepository.FindAsync(t => t.Id == parentTask.InstanceId && t.IsDisabled == false);
                if (subflowInstanceEntity == null)
                {
                    throw new InvalidOperationException("子流程数据丢失");
                }
                var parentWorkFlowInstalled = WorkFlowAnalyzing.WorkFlowInstalledDeserialize(subflowInstanceEntity.FlowRuntimeJson);
                if (parentWorkFlowInstalled != null)
                {
                    var parentStep = parentWorkFlowInstalled.GetStep(parentTask.StepId);

                    if (parentStep != null && parentStep.IsSubFlowStep() && !string.IsNullOrEmpty(parentStep.SubFlowFinishedEvent))
                    {
                        var eventArgs = new SubFlowFinishedEventArgs
                        {
                            FlowId = parentTask.FlowId,
                            GroupId = parentTask.GroupId,
                            InstanceId = parentTask.InstanceId,
                            StepId = parentTask.StepId,
                            TaskId = parentTask.Id,
                            SubFlowInstance = subflowInstance
                        };

                        var eventName = parentStep.SubFlowFinishedEvent.Trim();

                        var result = ExecuteCustomMethod<SubFlowFinishedEventArgs, SubFlowFinishedEventResults>(eventName, eventArgs);

                        if (result.IsError)
                        {
                            throw new InvalidOperationException(result.ErrorMessage);
                        }

                        return result;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 执行提交操作之前
        /// </summary>
        /// <returns></returns>
        private ExecuteSubmitBeforeEventResults ExecuteSubmitBeforeEvent(WorkFlowTask task, WorkFlowStep step)
        {
            if (!string.IsNullOrEmpty(step.SubmitBeforeEvent))
            {
                var eventName = step.SubmitBeforeEvent.Trim();
                var eventArgs = new ExecuteSubmitBeforeEventArgs()
                {
                    FlowId = task.FlowId,
                    GroupId = task.GroupId,
                    InstanceId = task.InstanceId,
                    StepId = task.StepId,
                    TaskId = task.Id
                };
                var result = ExecuteCustomMethod<ExecuteSubmitBeforeEventArgs, ExecuteSubmitBeforeEventResults>(eventName, eventArgs);

                if (result.IsError)
                {
                    throw new InvalidOperationException(result.ErrorMessage);
                }

                return result;
            }

            return null;
        }

        /// <summary>
        /// 执行提交操作之后
        /// </summary>
        /// <param name="task"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        private ExecuteSubmitAfterEventResults ExecuteSubmitAfterEvent(WorkFlowTask task, WorkFlowStep step)
        {
            if (!string.IsNullOrEmpty(step.SubmitAfterEvent))
            {
                var eventName = step.SubmitAfterEvent.Trim();
                var eventArgs = new ExecuteSubmitAfterEventArgs()
                {
                    FlowId = task.FlowId,
                    GroupId = task.GroupId,
                    InstanceId = task.InstanceId,
                    StepId = task.StepId,
                    TaskId = task.Id
                };
                var result = ExecuteCustomMethod<ExecuteSubmitAfterEventArgs, ExecuteSubmitAfterEventResults>(eventName, eventArgs);

                if (result.IsError)
                {
                    throw new InvalidOperationException(result.ErrorMessage);
                }

                return result;
            }

            return null;
        }

        /// <summary>
        /// 执行提交操作之前
        /// </summary>
        /// <returns></returns>
        private ExecuteBackBeforeEventResults ExecuteBackBeforeEvent(WorkFlowTask task, WorkFlowStep step)
        {
            if (!string.IsNullOrEmpty(step.BackBeforeEvent))
            {
                var eventName = step.BackBeforeEvent.Trim();
                var eventArgs = new ExecuteBackBeforeEventArgs()
                {
                    FlowId = task.FlowId,
                    GroupId = task.GroupId,
                    InstanceId = task.InstanceId,
                    StepId = task.StepId,
                    TaskId = task.Id
                };
                var result = ExecuteCustomMethod<ExecuteBackBeforeEventArgs, ExecuteBackBeforeEventResults>(eventName, eventArgs);

                if (result.IsError)
                {
                    throw new InvalidOperationException(result.ErrorMessage);
                }

                return result;
            }

            return null;
        }

        /// <summary>
        /// 执行提交操作之后
        /// </summary>
        /// <returns></returns>
        private ExecuteBackAfterEventResults ExecuteBackAfterEvent(WorkFlowTask task, WorkFlowStep step)
        {
            if (!string.IsNullOrEmpty(step.BackAfterEvent))
            {
                var eventName = step.BackAfterEvent.Trim();
                var eventArgs = new ExecuteBackAfterEventArgs()
                {
                    FlowId = task.FlowId,
                    GroupId = task.GroupId,
                    InstanceId = task.InstanceId,
                    StepId = task.StepId,
                    TaskId = task.Id
                };
                var result = ExecuteCustomMethod<ExecuteBackAfterEventArgs, ExecuteBackAfterEventResults>(eventName, eventArgs);

                if (result.IsError)
                {
                    throw new InvalidOperationException(result.ErrorMessage);
                }

                return result;
            }

            return null;
        }

        #endregion

        #region 条件处理

        public bool IsDataConditionPassed(JObject data, string condition)
        {
            bool isPassed = true;

            try
            {
                Regex reg = new Regex(@"{(\w+?)}");
                var matches = reg.Matches(condition);
                var query = condition;
                foreach (Match matche in matches)
                {
                    string field = matche.Value.Trim('{', '}').ToLower();

                    if (data[field] != null)
                    {
                        query = query.Replace(matche.Value, data[field].ToString());
                    }
                }
                isPassed = (bool)(new DataTable()).Compute(query, "");
            }
            catch
            {
                isPassed = false;
            }

            return isPassed;
        }

        public bool IsMethodConditionPassed(JObject data, string condition)
        {
            bool isPassed = true;
            try
            {
                isPassed = ExecuteCustomMethod<object, bool>(condition, data);
            }
            catch (System.Exception e)
            {
                isPassed = false;
                throw e;
            }
            return isPassed;
        }

        public bool IsSQLConditionPassed(JObject data, string condition)
        {
            bool isPassed = true;

            try
            {
                Regex reg = new Regex(@"{(\w+?)}");
                var matches = reg.Matches(condition);
                var query = condition;
                foreach (Match matche in matches)
                {
                    string field = matche.Value.Trim('{', '}').ToLower();

                    if (data[field] != null)
                    {
                        query = query.Replace(matche.Value, data[field].ToString());
                    }
                }

                isPassed = (Int64)adoNetDataTableGateway.ExecuteScalar(query) == 1;
            }
            catch (System.Exception e)
            {
                isPassed = false;
            }

            return isPassed;
        }

        #endregion

        #region 发送消息

        public void SendMessage(string title, string message, params Guid[] users)
        {
            if (users.Count() > 0)
            {
                eventBus.Publish<SendMessageEvent>(new SendMessageEvent
                {
                    Message = new Message
                    {
                        Title = $"【审批】{title}",
                        Content = $"【审批】{message}",
                        Level = 0
                    },
                    To = users
                });
            }
        }

        #endregion

        #region 获取任务

        /// <summary>
        /// 获取当前步骤分发的所有任务
        /// </summary>
        /// <returns></returns>
        private async Task<List<WorkFlowTask>> GetDistributionTask(Guid stepId, Guid flowId, Guid instanceId, Guid groupId, int sort)
        {
            var taskList = (await workFlowTaskRepository.FindAllAsync(t
                => t.FlowId == flowId
                && t.InstanceId == instanceId
                && t.StepId == stepId
                && t.GroupId == groupId
                && t.Sort == sort
                && t.State != (int)WorkFlowTaskKinds.Copy
                && t.IsDisabled == false)).ToList();

            return taskList;
        }

        /// <summary>
        /// 获取当前步骤最新分发的步骤
        /// </summary>
        /// <param name="stepId"></param>
        /// <param name="flowId"></param>
        /// <param name="instanceId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        private async Task<List<WorkFlowTask>> GetNewestDistributionTask(Guid stepId, Guid flowId, Guid instanceId, Guid groupId)
        {
            var taskList = (await workFlowTaskRepository.FindAllAsync(t
                => t.FlowId == flowId
                && t.InstanceId == instanceId
                && t.StepId == stepId
                && t.GroupId == groupId
                && t.State != (int)WorkFlowTaskKinds.Copy
                && t.IsDisabled == false)).ToList();

            var maxSort = taskList.OrderByDescending(t => t.Sort).Select(t => t.Sort).Max();

            return taskList.FindAll(t => t.Sort == maxSort);
        }

        /// <summary>
        /// 获取当前任务的所有同级步骤没有处理过的任务
        /// </summary>
        /// <param name="currentTask"></param>
        /// <returns></returns>
        private async Task<List<WorkFlowTask>> GetSameLevelStepNotExecuteTask(WorkFlowTask currentTask, WorkFlowExecute execute)
        {
            //获取下一步，反向推倒出
            var nextSteps = execute.WorkFlowInstalled.GetNextSteps(currentTask.StepId);

            var stepArray = new List<Guid>();

            foreach (var nextStep in nextSteps)
            {
                var nextStepPrevSteps = execute.WorkFlowInstalled.GetPrevSteps(nextSteps[0].StepId);
                stepArray.AddRange(nextStepPrevSteps.Select(t => t.StepId).ToList());
            }

            var list = await workFlowTaskRepository.FindAllAsync(t
                => stepArray.Contains(t.StepId)
                && t.InstanceId == currentTask.InstanceId
                && t.GroupId == currentTask.GroupId
                && t.FlowId == currentTask.FlowId
                && t.State != (int)WorkFlowTaskState.Handled
                && t.State != (int)WorkFlowTaskState.Returned
                && t.State != (int)WorkFlowTaskState.HandledByOthers
                && t.State != (int)WorkFlowTaskState.ReturnedByOthers
                && t.State != (int)WorkFlowTaskKinds.Copy
                && t.IsDisabled == false);

            return list.ToList();
        }

        /// <summary>
        /// 获取发起子流程的主流程任务
        /// </summary>
        /// <param name="subflowTask"></param>
        /// <returns></returns>
        private async Task<WorkFlowTask> GetParentTask(WorkFlowTask subflowTask)
        {
            var parentTask = await workFlowTaskRepository.FindAsync(t
                => t.GroupId == subflowTask.GroupId
                && t.SubFlowInstanceId == subflowTask.InstanceId
                && t.State != (int)WorkFlowTaskKinds.Copy
                && t.IsDisabled == false);

            return parentTask;
        }

        /// <summary>
        /// 获取一个实例所有的任务
        /// </summary>
        /// <returns></returns>
        private async Task<List<WorkFlowTask>> GetAllTask(WorkFlowInstance instance)
        {
            var list = await workFlowTaskRepository.FindAllAsync(t
                => t.InstanceId == instance.Id
                && t.FlowId == instance.FlowId
                && t.State != (int)WorkFlowTaskKinds.Copy
                && t.IsDisabled == false);

            return list.ToList();
        }

        #endregion

        public async Task<List<WorkFlowTask>> GetWorkFlowCommentsAsync(WorkFlowInstance instance)
        {
            var taskList = await workFlowTaskRepository.FindAllAsync(t 
                => t.InstanceId == instance.Id
                && t.FlowId == instance.FlowId
                && t.State != (int)WorkFlowTaskState.Waiting
                && t.CompanyId == instance.CompanyId
                && t.IsDisabled == false 
                && !string.IsNullOrEmpty(t.Comment),
            query => query.Desc(o => o.ExecutedTime));
            return taskList.ToList();
        }

        public async Task<List<WorkFlowTask>> GetWorkFlowProcessAsync(WorkFlowInstance instance)
        {
            var taskList = await workFlowTaskRepository.FindAllAsync(t 
                => t.InstanceId == instance.Id
                && t.FlowId == instance.FlowId
                && t.State != (int)WorkFlowTaskState.Waiting
                && t.CompanyId == t.CompanyId
                && t.IsDisabled == false,
            query => query.Desc(a => a.ExecutedTime));
            return taskList.ToList();
        }

        public async Task<Dictionary<int, List<WorkFlowStep>>> GetWorkFlowProcessStatesAsync(WorkFlowInstance instance)
        {
            Dictionary<int, List<WorkFlowStep>> result = new Dictionary<int, List<WorkFlowStep>>
            {
                { 0, new List<WorkFlowStep>() }, //已初始化
                { 1, new List<WorkFlowStep>() }, //等待中
                { 2, new List<WorkFlowStep>() }, //已挂起
                { 3, new List<WorkFlowStep>() } //已完成
            };

            var workFlowAllTask = await GetAllTask(instance);

            if (workFlowAllTask.Count > 0)
            {
                var groupId = workFlowAllTask.Select(t => t.GroupId).First();

                var workFlowInstalled = WorkFlowAnalyzing.WorkFlowInstalledDeserialize(instance.FlowRuntimeJson);

                foreach (var step in workFlowInstalled.Steps)
                {
                    var stepTasks = workFlowAllTask.Where(t => t.StepId == step.StepId);

                    if (stepTasks.Count() > 0)
                    {
                        int sort = workFlowAllTask.Where(t => t.StepId == step.StepId).Max(t => t.Sort);

                        if (await IsStepPassed(step, instance.Id, instance.FlowId, groupId, sort))
                        {
                            result[3].Add(step);
                        }
                        else if (await IsStepBacked(step, instance.Id, instance.FlowId, groupId, sort))
                        {
                            result[2].Add(step);
                        }
                        else
                        {
                            result[1].Add(step);
                        }
                    }
                    else
                    {
                        result[0].Add(step);
                    }
                }
            }

            return result;
        }
    }
}
