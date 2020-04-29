using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;
using Newtonsoft.Json.Linq;
using ZDY.DMS.Services.WorkFlowService.Core.Aspects;
using ZDY.DMS.Services.WorkFlowService.Core.Comparers;
using ZDY.DMS.Services.WorkFlowService.Core.Extensions;
using ZDY.DMS.Services.WorkFlowService.Core.Interfaces;
using ZDY.DMS.Services.WorkFlowService.Core.Models;
using ZDY.DMS.Services.WorkFlowService.Enums;
using ZDY.DMS.Services.WorkFlowService.Models;

namespace ZDY.DMS.Services.WorkFlowService.Core.Services
{
    public class WorkFlowExecutor : IWorkFlowExecutor
    {
        private readonly INoticeSender noticeSender;
        private readonly ITaskProvider taskProvider;
        private readonly IPersistenceProvider persistenceProvider;
        private readonly ISignatureProvider signatureProvider;

        public WorkFlowExecutor(INoticeSender noticeSender,
            ITaskProvider taskProvider,
            IPersistenceProvider persistenceProvider,
            ISignatureProvider signatureProvider)
        {
            this.noticeSender = noticeSender;
            this.taskProvider = taskProvider;
            this.persistenceProvider = persistenceProvider;
            this.signatureProvider = signatureProvider;
        }

        public async Task ExecuteAsync(WorkFlowExecute execute)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await HandleExecuteAsync(execute);

                scope.Complete();
            }
        }

        public async Task ExecuteStartAsync(WorkFlowInstance instance, Guid groupId)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await HandleExecuteStartAsync(instance, groupId);

                scope.Complete();
            }
        }

        public async Task HandleExecuteAsync(WorkFlowExecute execute)
        {
            var task = await HandleGetWorkFlowTaskAsync(execute.TaskId);

            var instance = await HandleGetWorkFlowInstanceAsync(task.InstanceId);

            var definition = WorkFlowDefinition.Parse(instance.FlowRuntimeJson);

            var step = definition.GetStep(task.StepId);

            //创建处理上下文
            var execution = new WorkFlowExecution
            {
                Definition = definition,
                Instance = instance,
                GroupId = task.GroupId,
                Task = task,
                Step = step,
                Sender = execute.Sender,
                ToStepCollection = execute.Steps,
                Comment = execute.Comment,
                Note = execute.Note
            };

            //审批签名检测
            if (execution.Step.IsNeedSignature())
            {
                if (!await this.signatureProvider.TrySignatureAsync(execute.Sender.Id, execute.SignaturePassword))
                {
                    throw new InvalidOperationException("密钥错误或其他原因导致签名失败");
                }
            }

            if (execute.IsSubmit())
            {
                ExecuteInterceptor(execution.Step.BeforeSubmitInterceptor, execution.Task);
                await ExecuteSubmit(execution);
                ExecuteInterceptor(execution.Step.AfterSubmitInterceptor, execution.Task);
            }
            else if (execute.IsBack())
            {
                ExecuteInterceptor(execution.Step.BeforeBackInterceptor, execution.Task);
                await ExecuteBack(execution);
                ExecuteInterceptor(execution.Step.AfterBackInterceptor, execution.Task);
            }
            else if (execute.IsRedirect())
            {
                await ExecuteRedirect(execution);
            }
            else
            {
                throw new InvalidOperationException();
            }

            //更新实例中步骤信息
            await UpdateWorkFlowInstanceLastExecuteTask(execution.Instance, execution.Task);
        }

        public async Task HandleExecuteStartAsync(WorkFlowInstance instance, Guid groupId)
        {
            var flow = await HandleGetWorkFlowAsync(instance.FlowId);

            var definition = WorkFlowDefinition.Parse(flow.RuntimeJson);

            //初始化开始节点和结束节点的一些默认规则
            foreach (var step in definition.Steps)
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

            instance.Id = Guid.NewGuid();
            instance.FlowName = flow.Name;
            instance.FlowRuntimeJson = definition.ToString();
            instance.FlowDesignJson = flow.DesignJson;

            //创建实例
            await this.persistenceProvider.CreateInstanceAsync(instance);

            //创建开始任务
            var startStep = definition.GetStart();

            var startTask = new WorkFlowTask();

            startTask.Title = string.IsNullOrEmpty(instance.Title) ? "未命名任务(" + definition.Name + ")" : instance.Title;
            startTask.FlowId = flow.Id;
            startTask.FlowName = flow.Name;
            startTask.GroupId = groupId;
            startTask.Type = (int)WorkFlowTaskKinds.Normal;
            startTask.InstanceId = instance.Id;
            startTask.Note = "";
            startTask.PrevTaskId = default;
            startTask.PrevStepId = default;
            startTask.ReceiverId = instance.CreaterId;
            startTask.ReceiverName = instance.CreaterName;
            startTask.ReceiveTime = DateTime.Now;
            startTask.SenderId = instance.CreaterId;
            startTask.SenderName = instance.CreaterName;
            startTask.SendTime = startTask.ReceiveTime;
            startTask.State = (int)WorkFlowTaskState.Pending;
            startTask.StepId = startStep.StepId;
            startTask.StepName = startStep.StepName;
            startTask.Sort = 1;
            startTask.CompanyId = instance.CompanyId;

            if (startStep.TimeLimit > 0)
            {
                startTask.PlannedTime = DateTime.Now.AddHours((double)startStep.TimeLimit);
            }

            await this.persistenceProvider.CreateTaskAsync(startTask);

            //更新实例中步骤信息
            await UpdateWorkFlowInstanceLastExecuteTask(instance, startTask);
        }

        #region 安全的获取基础数据

        private async Task<WorkFlowInstance> HandleGetWorkFlowInstanceAsync(Guid instanceId)
        {
            var instance = await this.persistenceProvider.GetWorkFlowInstanceAsync(instanceId);

            if (instance == null)
            {
                throw new InvalidOperationException("流程实例数据丢失");
            }

            return instance;
        }

        private async Task<WorkFlowTask> HandleGetWorkFlowTaskAsync(Guid taskId)
        {
            if (taskId.Equals(default))
            {
                throw new InvalidOperationException("未能创建或找到当前任务");
            }

            var task = await this.persistenceProvider.GetWorkFlowTaskAsync(taskId);

            if (task == null)
            {
                throw new InvalidOperationException("未能创建或找到当前任务");
            }

            if (task.IsExecute())
            {
                throw new InvalidOperationException("当前任务已处理完成");
            }

            return task;
        }

        private async Task<WorkFlow> HandleGetWorkFlowAsync(Guid flowId)
        {
            if (flowId.Equals(default))
            {
                throw new InvalidOperationException("流程数据未找到，发起流程失败");
            }

            var flow = await this.persistenceProvider.GetWorkFlowAsync(flowId);

            if (flow == null || flow.IsNotInstalled())
            {
                throw new InvalidOperationException("流程数据未找到或流程未安装");
            }

            return flow;
        }

        private async Task<WorkFlowForm> HandleGetWorkFlowFormAsync(Guid formId)
        {
            if (formId.Equals(default))
            {
                throw new InvalidOperationException("表单数据未找到，发起流程失败");
            }

            var form = await this.persistenceProvider.GetWorkFlowFormAsync(formId);

            if (form == null || form.IsNotPublished())
            {
                throw new InvalidOperationException("表单数据未找到或表单未发布");
            }

            return form;

        }

        #endregion

        #region 流程实例相关

        /// <summary>
        /// 结束流程实例
        /// </summary>
        /// <returns></returns>
        private async Task FinishWorkFlowInstance(WorkFlowInstance instance, WorkFlowTask endStepTask)
        {
            await this.persistenceProvider.UpdateInstanceAsync(instance);

            //如果有临时任务，直接删除掉
            await RemoveTemporaryTask(instance.Id);

            //如果当前任务如果是个包含子流程任务的任务
            await HandleSubFlowFinished(endStepTask);

            //发送消息
            SendMessage(instance.Title, $"<b>{instance.Title}</b>审批结束！", instance.CreaterId);
        }

        /// <summary>
        /// 子流程结束
        /// </summary>
        /// <returns></returns>
        private async Task HandleSubFlowFinished(WorkFlowTask subflowEndStepTask)
        {
            // 寻找出发子流程的根任务
            var rootInstanceTask = await this.persistenceProvider.GetRootInstanceTaskAsync(subflowEndStepTask.InstanceId, subflowEndStepTask.GroupId);

            // 执行子流程完成后事件
            if (rootInstanceTask != null)
            {
                var rootInstance = await HandleGetWorkFlowInstanceAsync(rootInstanceTask.InstanceId);

                var rootWorkFlowDefinition = WorkFlowDefinition.Parse(rootInstance.FlowRuntimeJson);

                if (rootWorkFlowDefinition != null)
                {
                    var rootInstanceTaskStep = rootWorkFlowDefinition.GetStep(rootInstanceTask.StepId);

                    if (rootInstanceTaskStep != null && rootInstanceTaskStep.IsSubFlowStep())
                    {
                        ExecuteInterceptor(rootInstanceTaskStep.SubFlowFinishedInterceptor, rootInstanceTask);
                    }
                }
            }
        }

        /// <summary>
        /// 成功结束流程实例
        /// </summary>
        /// <returns></returns>
        private async Task CompleteWorkFlowInstance(WorkFlowInstance instance, WorkFlowTask endStepTask)
        {
            instance.State = (int)WorkFlowInstanceState.Completed;

            await FinishWorkFlowInstance(instance, endStepTask);
        }

        /// <summary>
        /// 意外关闭流程实例
        /// </summary>
        /// <returns></returns>
        private async Task CloseWorkFlowInstance(WorkFlowInstance instance, WorkFlowTask endStepTask)
        {
            instance.State = (int)WorkFlowInstanceState.Closed;

            await FinishWorkFlowInstance(instance, endStepTask);
        }

        /// <summary>
        /// 更新流程进度信息
        /// </summary>
        /// <param name="instanceId"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        private async Task UpdateWorkFlowInstanceLastExecuteTask(WorkFlowInstance instance, WorkFlowTask lastExecuteTask)
        {

            instance.LastExecuteTaskId = lastExecuteTask.Id;
            instance.LastExecuteStepId = lastExecuteTask.StepId;
            instance.LastExecuteStepName = lastExecuteTask.StepName;
            instance.LastModifyTime = DateTime.Now;

            await this.persistenceProvider.UpdateInstanceAsync(instance);
        }

        #endregion

        #region 获取接收者 

        /// <summary>
        /// 获取用户
        /// </summary>
        private List<WorkFlowUser> GetUser(Guid companyId, Guid[] userArray)
        {
            if (userArray?.Count() <= 0)
            {
                return new List<WorkFlowUser>();
            }

            return new List<WorkFlowUser>();
        }

        /// <summary>
        /// 获得抄送任务接收者
        /// </summary>
        private List<WorkFlowUser> GetCopyTaskReceivers(WorkFlowExecution execution)
        {
            var receivers = execution.Step.CopyToUsers.Split(',').Select(t => Guid.Parse(t)).ToArray();

            return GetUser(execution.Instance.CompanyId, receivers);
        }

        /// <summary>
        /// 获取转交任务接收者
        /// </summary>
        /// <param name="execution"></param>
        /// <returns></returns>
        private List<WorkFlowUser> GetRedirectTaskReceivers(WorkFlowExecution execution)
        {
            var receivers = execution.ToStepCollection.First().Value.Select(t => t.Id).ToArray();

            return GetUser(execution.Instance.CompanyId, receivers);
        }

        #endregion

        #region 处理方式

        /// <summary>
        /// 提交任务
        /// </summary>
        /// <param name="execution"></param>
        /// <returns></returns>
        private async Task ExecuteSubmit(WorkFlowExecution execution)
        {
            //如果当前步骤是子流程步骤，并且策略是【子流程完成后才能提交】,则要判断子流程是否已完成
            if (execution.Step.IsSubFlowStep())
            {
                if (!await IsSubFlowStepPassed(execution))
                {
                    throw new InvalidOperationException("当前步骤的子流程未完成，不能提交");
                }
            }

            //判断控制规则，验证步骤
            HandleWorkFlowControl(execution);

            //获取步骤处理人,在步骤中加入处理人
            HandleNextStepReceivers(execution);

            //处理策略判断，返回下一步是否需要等待
            bool isNeedNextStepTaskWating = await HandleSubmitTactic(execution);

            //如果存在下一步骤，如果不存在下一步，则判断是否是最后一步
            if (execution.ToStepCollection.Any())
            {
                if (isNeedNextStepTaskWating)
                {
                    //如果需要等待，说明多人步骤中有人存在待处理任务，则为当前用户创建一个状态为等待中的后续任务，等条件满足后才修改状态，待办人员看不到。
                    await CreateTemporaryTask(execution);
                }
                else
                {
                    //创建下一步任务
                    //如果存在会签任务，则需要判断会签，如果会签没有通过，则还需要继续等待
                    //如果下一步任务创建成功，则之前创建的临时任务将执行激活
                    var nextStepTasks = await CreateNextStepTask(execution);

                    //下一步任务数大于 0，说明任务已经进行到下一步，则创建抄送任务
                    if (nextStepTasks.Count > 0)
                    {
                        //发送消息
                        SendMessage(execution.Instance.Title, $"您有一个新的审批需要处理，关于<b>{execution.Instance.Title}</b>.", nextStepTasks.Select(t => t.ReceiverId).Distinct().ToArray());

                        await CreateCopyTask(execution);
                    }
                    else
                    {
                        //下一步任务没有创建，说明还在等待会签中，则创建下一步的临时任务
                        await CreateTemporaryTask(execution);
                    }
                }
            }
            else
            {
                //如果是最后一步，则检查当前步骤是否全部完成，如果完成，将结束流程
                if (execution.Step.IsEnd())
                {
                    //如果结束步骤完成，则更新实例状态为已完成
                    var isComplated = await IsStepPassed(execution.Step, execution.Task.InstanceId, execution.Task.Sort);

                    if (isComplated)
                    {
                        //结束流程
                        await CompleteWorkFlowInstance(execution.Instance, execution.Task);
                    }
                }
            }

            //发送信息
            SendMessage(execution.Instance.Title, $"<b>{execution.Sender.Name}</b>处理了<b>{execution.GetTaskTitle()}</b>的<b>{execution.Step.StepName}</b>.", execution.Instance.CreaterId);
        }

        /// <summary>
        /// 退回任务
        /// </summary>
        /// <param name="execution"></param>
        private async Task ExecuteBack(WorkFlowExecution execution)
        {
            if (execution.Step.IsBackTacticBy(WorkFlowBackTacticKinds.UnableToReturn))
            {
                throw new InvalidOperationException("当前步骤不能退回");
            }

            //回退策略判断，返回下一步是否需要等待
            bool isNextStepTaskWating = await ExecuteBackBackTactic(execution);

            if (execution.ToStepCollection.Any())
            {
                if (!isNextStepTaskWating)
                {
                    //创建回退任务
                    var backStepTasks = await CreateBackStepTask(execution);

                    if (backStepTasks.Count > 0)
                    {
                        //发送消息
                        SendMessage(execution.Instance.Title, $"<b>{execution.Instance.Title}</b>的<b>{execution.Step.StepName}</b>被退回，需要您重新处理.", backStepTasks.Select(t => t.ReceiverId).Distinct().ToArray());
                    }
                }
            }
            else
            {
                //如果是第一步，则检查当前步骤判断策略是退回，将结束流程
                if (execution.Step.IsStart())
                {
                    //如果结束步骤完成，则更新实例状态为已完成
                    var isBacked = await IsStepBacked(execution.Step, execution.Task.InstanceId, execution.Task.Sort);

                    if (isBacked)
                    {
                        //结束流程,属于创建人自己退回，就关闭掉
                        await CloseWorkFlowInstance(execution.Instance, execution.Task);
                    }
                }
            }

            //发送信息
            SendMessage(execution.Instance.Title, $"<b>{execution.Sender.Name}</b>退回了<b>{execution.GetTaskTitle()}</b>的<b>{execution.Step.StepName}</b>.", execution.Instance.CreaterId);

        }

        /// <summary>
        /// 转交任务
        /// </summary>
        /// <param name="executionModel"></param>
        private async Task ExecuteRedirect(WorkFlowExecution execution)
        {
            if (execution.Task.Is(WorkFlowTaskState.Waiting))
            {
                throw new InvalidOperationException("当前任务正在等待他人处理");
            }

            if (!execution.ToStepCollection.Any())
            {
                throw new InvalidOperationException("未或者转交任务");
            }

            if (execution.ToStepCollection.First().Value.Count == 0)
            {
                throw new InvalidOperationException("未设置转交人员");
            }

            //获取步骤任务接收者
            var receivers = GetRedirectTaskReceivers(execution);

            execution.ToStepCollection.First().Value.Clear();

            execution.ToStepCollection.First().Value.AddRange(receivers);


            var redirectTasks = new List<WorkFlowTask>();

            foreach (var receiver in receivers)
            {
                if (await IsHasNotExecuteTask(execution.Task.InstanceId, execution.Task.StepId, receiver.Id))
                {
                    continue;
                }

                WorkFlowTask task = new WorkFlowTask();

                task.PlannedTime = execution.Task.PlannedTime;
                task.FlowId = execution.Task.FlowId;
                task.FlowName = execution.Task.FlowName;
                task.GroupId = execution.Task.GroupId;
                task.InstanceId = execution.Task.InstanceId;
                task.PrevTaskId = execution.Task.PrevTaskId;
                task.PrevStepId = execution.Task.PrevStepId;
                task.ReceiveTime = execution.Task.ReceiveTime;
                task.SenderId = execution.Task.SenderId;
                task.SenderName = execution.Task.SenderName;
                task.SendTime = execution.Task.SendTime;
                task.StepId = execution.Task.StepId;
                task.StepName = execution.Task.StepName;
                task.Sort = execution.Task.Sort;
                task.Title = execution.Task.Title;
                task.CompanyId = execution.Task.CompanyId;

                task.ReceiverId = receiver.Id;
                task.ReceiverName = receiver.Name;
                task.State = (int)WorkFlowTaskState.Pending;
                task.Type = (int)WorkFlowTaskKinds.Redirect;
                task.Note = $"该由 {execution.Task.ReceiverName} 转交";

                await this.persistenceProvider.CreateTaskAsync(task);

                redirectTasks.Add(task);
            }

            await HandleUpdateWorkFlowTaskStateAsync(execution.Task, execution.Comment, execution.Step.IsNeedSignature(), WorkFlowTaskState.Handled, "已转交他人处理");

            if (redirectTasks.Count > 0)
            {
                //发送消息
                SendMessage(execution.Instance.Title, $"<b>{execution.Sender.Name}</b>将<b>{execution.Step.StepName}</b>转交给您审批.", redirectTasks.Select(t => t.ReceiverId).Distinct().ToArray());

                string receiveNames = string.Join(",", redirectTasks.Select(t => t.ReceiverName).Distinct().ToArray());

                //发送信息
                SendMessage(execution.Instance.Title, $"<b>{execution.Sender.Name}</b>将<b>{execution.Step.StepName}</b>转交给<b>{receiveNames}</b>处理.", execution.Instance.CreaterId);
            }

        }

        #endregion

        #region 提交任务相关规则判断

        /// <summary>
        /// 判断处理步骤的控制规则，选择性加入步骤
        /// </summary>
        /// <param name="execution"></param>
        /// <param name="current"></param>
        private void HandleWorkFlowControl(WorkFlowExecution execution)
        {
            var nextSteps = execution.Definition.GetNextSteps(execution.Step.StepId);

            //是否下一步有选择任意人员
            if (nextSteps.Any(t => t.IsHandleBy(WorkFlowHandlerKinds.AnyUser)))
            {
                //如果没选择步骤，提示
                if (!execution.ToStepCollection.Any())
                {
                    throw new InvalidOperationException("未选择下一步步骤");
                }

                foreach (var step in execution.ToStepCollection)
                {
                    var nextStep = execution.Definition.GetStep(step.Key);

                    if (nextStep.IsHandleBy(WorkFlowHandlerKinds.AnyUser))
                    {
                        if (step.Value == null || step.Value.Where(t => !t.Id.Equals(default)).Count() == 0)
                        {
                            throw new InvalidOperationException("有步骤没有选择处理人员");
                        }
                    }
                }
            }

            //判断流程控制
            switch ((WorkFlowControlKinds)execution.Step.FlowControl)
            {
                case WorkFlowControlKinds.System:

                    foreach (var nextStep in nextSteps)
                    {
                        if (!execution.ToStepCollection.ContainsKey(nextStep.StepId))
                        {
                            execution.ToStepCollection.Add(nextStep.StepId, new List<WorkFlowUser>());
                        }
                    }

                    //判断条件通过情况
                    HandleTransits(execution);

                    break;
                case WorkFlowControlKinds.SingleSelect:

                    if (execution.ToStepCollection.Count() != 1)
                    {
                        throw new InvalidOperationException("当前步骤必须只能选择一个步骤提交");
                    }

                    break;
                case WorkFlowControlKinds.MultiSelect:

                    //处理步骤如果不是最后一步的提交任务，则检查处理步骤的下一步是否存在
                    if (!execution.Step.IsEnd())
                    {
                        if (execution.ToStepCollection.Count == 0)
                        {
                            throw new InvalidOperationException("请选择步骤后提交");
                        }
                    }

                    break;
            }
        }

        /// <summary>
        /// 由系统控制时，过滤满足条件的步骤
        /// </summary>
        private void HandleTransits(WorkFlowExecution execution)
        {
            var stepBuilder = new Dictionary<Guid, List<WorkFlowUser>>();

            //获取数据
            JObject data = JObject.Parse(execution.Instance.FormDataJson);

            var formStep = execution.Step;

            foreach (var toStep in execution.ToStepCollection)
            {
                var transit = execution.Definition.GetTransit(formStep.StepId, toStep.Key);

                if (transit != null)
                {
                    if (transit.IsConditionPassed(data))
                    {
                        stepBuilder.Add(toStep.Key, toStep.Value);
                    }
                }
            }

            execution.ToStepCollection = stepBuilder;
        }

        /// <summary>
        /// 处理提交任务时的处理策略
        /// </summary>
        /// <returns>下一步任务是否需要等待状态</returns>
        private async Task<bool> HandleSubmitTactic(WorkFlowExecution execution)
        {
            bool isNeedWating = false;

            //第一步和最后一步只有发起者处理，不判断策略
            if (execution.Task.IsStart() || execution.Task.IsEnd())
            {
                await HandleUpdateWorkFlowTaskStateAsync(execution.Task, execution.Comment, execution.Step.IsNeedSignature(), WorkFlowTaskState.Handled);
            }
            else
            {
                string note = "";

                //获取当前步骤分发的任务
                var taskList = await this.taskProvider.GetDistributionTaskAsync(execution.Task.InstanceId, execution.Task.Sort, execution.Step.StepId);

                switch ((WorkFlowHandleTacticKinds)execution.Step.HandleTactic)
                {
                    case WorkFlowHandleTacticKinds.AllAgree://所有人必须处理

                        if (taskList.Count > 1)
                        {
                            var noCompleted = taskList.FindAll(t => t.IsNot(WorkFlowTaskState.Handled));

                            if (noCompleted.Count() - 1 > 0)
                            {
                                isNeedWating = true;
                            }
                        }

                        note = isNeedWating ? $"当前步骤已处理完成，但还需等待其他人处理" : "";

                        await HandleUpdateWorkFlowTaskStateAsync(execution.Task, execution.Comment, execution.Step.IsNeedSignature(), WorkFlowTaskState.Handled, note);

                        break;

                    case WorkFlowHandleTacticKinds.OneAgree://一人同意即可

                        foreach (var task in taskList)
                        {
                            if (task.Id != execution.Task.Id)
                            {
                                if (task.IsNotExecute())
                                {
                                    note = $"当前步骤只需一人同意即可抵达至下一步，其中 {execution.Sender.Name} 已批示同意";

                                    await HandleUpdateWorkFlowTaskStateAsync(task, "", false, WorkFlowTaskState.HandledByOthers, note);
                                }
                            }
                            else
                            {
                                await HandleUpdateWorkFlowTaskStateAsync(task, execution.Comment, execution.Step.IsNeedSignature(), WorkFlowTaskState.Handled);
                            }
                        }

                        break;

                    case WorkFlowHandleTacticKinds.PercentageAgree://依据人数比例

                        if (taskList.Count > 1)
                        {
                            decimal percentage = execution.Step.Percentage <= 0 ? 100 : execution.Step.Percentage;//比例

                            decimal nextPercentage = Math.Round((((decimal)(taskList.Where(t => t.Is(WorkFlowTaskState.Handled)).Count() + 1) / (decimal)taskList.Count) * 100));

                            if (nextPercentage < percentage)
                            {
                                isNeedWating = true;
                            }
                            else
                            {
                                foreach (var task in taskList)
                                {
                                    if (task.Id != execution.Task.Id && task.IsNotExecute())
                                    {
                                        note = $"当前步骤有 {nextPercentage}% 的人批示同意，高于设定比例 {percentage}% ，遂由系统自动判定此步骤通过";

                                        await HandleUpdateWorkFlowTaskStateAsync(task, "", false, WorkFlowTaskState.HandledByOthers, note);
                                    }
                                }
                            }
                        }

                        note = isNeedWating ? $"当前步骤已处理完成，但还需等待其他人处理" : "";

                        await HandleUpdateWorkFlowTaskStateAsync(execution.Task, execution.Comment, execution.Step.IsNeedSignature(), WorkFlowTaskState.Handled, note);

                        break;

                    case WorkFlowHandleTacticKinds.Independent://独立处理

                        await HandleUpdateWorkFlowTaskStateAsync(execution.Task, execution.Comment, execution.Step.IsNeedSignature(), WorkFlowTaskState.Handled);

                        break;
                }
            }

            return isNeedWating;
        }

        #endregion

        #region 下一步任务

        /// <summary>
        /// 处理下一步任务接收者
        /// </summary>
        private void HandleNextStepReceivers(WorkFlowExecution execution)
        {
            var nextStepBuilder = new Dictionary<Guid, List<WorkFlowUser>>();

            foreach (var item in execution.ToStepCollection)
            {
                var appointReceivers = item.Value.Select(t => t.Id).ToList();

                var toStep = execution.Definition.GetStep(item.Key);

                if (!string.IsNullOrWhiteSpace(toStep.Handlers))
                {
                    var toStepReceivers = toStep.Handlers.Split(',').Select(t => Guid.Parse(t)).ToList();

                    appointReceivers.AddRange(toStepReceivers);
                }

                List<WorkFlowUser> receivers = GetUser(execution.Task.CompanyId, appointReceivers.Distinct().ToArray());

                //如果步骤没有处理人，则忽略
                if (receivers.Count > 0)
                {
                    nextStepBuilder.Add(item.Key, receivers);
                }
            }

            execution.ToStepCollection = nextStepBuilder;
        }

        /// <summary>
        /// 创建下一步任务，如果任务存在等待状态将执行激活
        /// </summary>
        /// <returns></returns>
        private async Task<List<WorkFlowTask>> CreateNextStepTask(WorkFlowExecution execution)
        {
            var nextStepTasks = new List<WorkFlowTask>();

            foreach (var step in execution.ToStepCollection)
            {
                // 步骤如果不存在，直接忽略
                var nextStep = execution.Definition.GetStep(step.Key);

                if (nextStep == null)
                {
                    continue;
                }

                foreach (var nextStepHandler in step.Value)
                {
                    // 处理下一步会签
                    bool isPass = await HandleCountersignatureStepNext(nextStep, execution);

                    if (isPass)
                    {
                        // 检查当前用户在此步骤是否已被创建任务
                        if (await IsHasNotExecuteTask(execution.Task.InstanceId, nextStep.StepId, nextStepHandler.Id))
                        {
                            return null;
                        }

                        var createdNextStepTask = await HandleCreateNextStepTask(nextStep, nextStepHandler, execution);

                        nextStepTasks.Add(createdNextStepTask);
                    }
                }
            }

            if (nextStepTasks.Count > 0)
            {
                await HandleTemporaryTaskNext(nextStepTasks);
            }

            return nextStepTasks;
        }

        /// <summary>
        /// 实施创建下一步任务
        /// </summary>
        /// <returns></returns>
        private async Task<WorkFlowTask> HandleCreateNextStepTask(WorkFlowStep nextStep, WorkFlowUser nextStepHandler, WorkFlowExecution execution)
        {
            var task = new WorkFlowTask();

            task.FlowId = execution.Task.FlowId;
            task.FlowName = execution.Task.FlowName;
            task.GroupId = execution.Task.GroupId;
            task.Type = (int)WorkFlowTaskKinds.Normal;
            task.InstanceId = execution.Task.InstanceId;
            task.Note = execution.Note;
            task.PrevTaskId = execution.Task.Id;
            task.PrevStepId = execution.Task.StepId;
            task.ReceiverId = nextStepHandler.Id;
            task.ReceiverName = nextStepHandler.Name;
            task.ReceiveTime = DateTime.Now;
            task.SenderId = execution.Sender.Id;
            task.SenderName = execution.Sender.Name;
            task.SendTime = task.ReceiveTime;
            task.State = (int)WorkFlowTaskState.Pending;
            task.StepId = nextStep.StepId;
            task.StepName = nextStep.StepName;
            task.Sort = execution.Task.Sort + 1;
            task.Title = execution.GetTaskTitle();
            task.CompanyId = execution.Task.CompanyId;

            if (nextStep.TimeLimit > 0)
            {
                task.PlannedTime = DateTime.Now.AddHours((double)nextStep.TimeLimit);
            }

            // 如果当前步骤是子流程步骤，则要发起子流程实例
            if (nextStep.IsSubFlowStep() && !string.IsNullOrEmpty(nextStep.SubFlowId))
            {
                //生成子流程实例Id
                task.SubFlowInstanceId = Guid.NewGuid();

                await HandleSubFlowStepNext(task, nextStep, nextStepHandler);
            }

            await this.persistenceProvider.CreateTaskAsync(task);

            return task;
        }

        /// <summary>
        /// 如果下一步为子流程步骤，则创建子流程实例
        /// </summary>
        /// <returns></returns>
        private async Task HandleSubFlowStepNext(WorkFlowTask subflowTask, WorkFlowStep subflowStep, WorkFlowUser sender)
        {
            var subflowId = Guid.Parse(subflowStep.SubFlowId);

            var subflow = await HandleGetWorkFlowAsync(subflowId);

            var subflowForm = await HandleGetWorkFlowFormAsync(subflow.FormId);

            //创建子流程实例
            var subflowInstance = new WorkFlowInstance
            {
                Id = subflowTask.InstanceId,
                FormJson = subflowForm.DesignJson,
                FormDataJson = "",
                Title = $"因任务【{subflowTask.Title}】创建的子流程审批",
                FlowId = Guid.Parse(subflowStep.SubFlowId),
                CreaterId = sender.Id,
                CreaterName = sender.Name,
                CompanyId = subflowTask.CompanyId,
                IsDisabled = false
            };

            //执行子流程开启前
            ExecuteInterceptor(subflowStep.BeforeSubFlowActivationInterceptor, subflowTask);

            //执行启动子流程
            await ExecuteStartAsync(subflowInstance, subflowTask.GroupId);

            //执行子流程开启后
            ExecuteInterceptor(subflowStep.AfterSubFlowActivationInterceptor, subflowTask);
        }

        /// <summary>
        /// 处理下一步会签
        /// </summary>
        /// <returns></returns>
        private async Task<bool> HandleCountersignatureStepNext(WorkFlowStep nextStep, WorkFlowExecution execution)
        {
            // 判断下一步是否需要会签
            bool isPass = nextStep.IsNotNeedCountersignature();

            // 如果下一步骤需要会签，则将未处理的其他步骤的任务全部处理掉
            if (nextStep.IsNeedCountersignature())
            {
                isPass = await IsCountersignatureStepPassed(nextStep, execution);

                if (isPass)
                {
                    // 会签通过，处理会签节点之前的所有需要会签节点任务
                    var sameLevelStepTaskList = await HandleGetSameLevelStepNotExecuteTask(execution);

                    foreach (var task in sameLevelStepTaskList)
                    {
                        if (task.Id == execution.Task.Id || task.IsExecute())
                        {
                            continue;
                        }

                        await HandleUpdateWorkFlowTaskStateAsync(task, "", false, WorkFlowTaskState.HandledByOthers, "会签已结束，此步骤因其他人员审批通过");
                    }
                }
            }

            return isPass;
        }

        /// <summary>
        /// 处理下一步临时任务，查找顺便将等待中的任务激活
        /// </summary>
        /// <param name="nextStepTasks"></param>
        /// <returns></returns>
        private async Task HandleTemporaryTaskNext(List<WorkFlowTask> nextStepTasks)
        {
            //激活临时任务
            var stepArray = nextStepTasks.Select(t => t.StepId).Distinct().ToArray();

            var nextStepTask = nextStepTasks.First();

            await ActivateTemporaryTask(nextStepTask.InstanceId, nextStepTask, stepArray);
        }

        #endregion

        #region  抄送任务

        /// <summary>
        /// 创建抄送任务
        /// </summary>
        /// <param name="currentTask"></param>
        /// <param name="execution"></param>
        /// <returns></returns>
        private async Task CreateCopyTask(WorkFlowExecution execution)
        {
            foreach (var step in execution.ToStepCollection)
            {
                var nextStep = execution.Definition.GetStep(step.Key);

                if (nextStep == null)
                {
                    continue;
                }

                if (nextStep.IsNeedCopy())
                {
                    var receivers = GetCopyTaskReceivers(execution);

                    foreach (var receiver in receivers)
                    {
                        if (await IsHasNotExecuteTask(execution.Task.InstanceId, nextStep.StepId, receiver.Id))
                        {
                            continue;
                        }

                        await HandleCreateCopyTask(nextStep, receiver, execution);
                    }
                }
            }
        }

        private async Task<WorkFlowTask> HandleCreateCopyTask(WorkFlowStep nextStep, WorkFlowUser receiver, WorkFlowExecution execution)
        {
            var task = new WorkFlowTask();

            task.FlowId = execution.Task.FlowId;
            task.FlowName = execution.Task.FlowName;
            task.GroupId = execution.Task.GroupId;
            task.Type = (int)WorkFlowTaskKinds.Copy;
            task.InstanceId = execution.Task.InstanceId;
            task.Note = string.IsNullOrWhiteSpace(execution.Note) ? "抄送任务" : execution.Note + "(抄送任务)";
            task.PrevTaskId = execution.Task.Id;
            task.PrevStepId = execution.Task.StepId;
            task.ReceiverId = receiver.Id;
            task.ReceiverName = receiver.Name;
            task.ReceiveTime = DateTime.Now;
            task.SenderId = execution.Sender.Id;
            task.SenderName = execution.Sender.Name;
            task.SendTime = task.ReceiveTime;
            task.State = (int)WorkFlowTaskState.Pending;
            task.StepId = nextStep.StepId;
            task.StepName = nextStep.StepName;
            task.Sort = execution.Task.Sort + 1;
            task.Title = execution.GetTaskTitle();
            task.CompanyId = execution.Task.CompanyId;

            if (nextStep.TimeLimit > 0)
            {
                task.PlannedTime = DateTime.Now.AddHours((double)nextStep.TimeLimit);
            }

            await this.persistenceProvider.CreateTaskAsync(task);

            return task;
        }

        #endregion

        #region 临时任务

        /// <summary>
        /// 创建临时任务
        /// </summary>
        private async Task<List<WorkFlowTask>> CreateTemporaryTask(WorkFlowExecution execution)
        {
            List<WorkFlowTask> temporaryTask = new List<WorkFlowTask>();

            foreach (var step in execution.ToStepCollection)
            {
                var nextStep = execution.Definition.GetStep(step.Key);

                if (nextStep == null)
                {
                    continue;
                }

                foreach (var nextStepHandler in step.Value)
                {
                    // 如果某个处理者没有处理当前步骤的任务，则不创建临时任务
                    if (await IsHasNotExecuteTask(execution.Task.InstanceId, nextStep.StepId, nextStepHandler.Id))
                    {
                        continue;
                    }

                    var task = await HandleCreateTemporaryTask(nextStep, nextStepHandler, execution);

                    temporaryTask.Add(task);
                }
            }

            return temporaryTask;
        }

        /// <summary>
        /// 实施创建临时任务
        /// </summary>
        private async Task<WorkFlowTask> HandleCreateTemporaryTask(WorkFlowStep nextStep, WorkFlowUser nextStepHandler, WorkFlowExecution execution)
        {
            var task = new WorkFlowTask();

            task.FlowId = execution.Task.FlowId;
            task.FlowName = execution.Task.FlowName;
            task.GroupId = execution.Task.GroupId;
            task.Type = (int)WorkFlowTaskKinds.Normal;
            task.InstanceId = execution.Task.InstanceId;
            task.Note = execution.Note;
            task.PrevTaskId = execution.Task.Id;
            task.PrevStepId = execution.Task.StepId;
            task.ReceiverId = nextStepHandler.Id;
            task.ReceiverName = nextStepHandler.Name;
            task.ReceiveTime = DateTime.Now;
            task.SenderId = execution.Sender.Id;
            task.SenderName = execution.Sender.Name;
            task.SendTime = task.ReceiveTime;
            task.State = (int)WorkFlowTaskState.Waiting;
            task.StepId = nextStep.StepId;
            task.StepName = nextStep.StepName;
            task.Sort = execution.Task.Sort + 1;
            task.Title = execution.GetTaskTitle();
            task.CompanyId = execution.Task.CompanyId;

            if (nextStep.TimeLimit > 0)
            {
                task.PlannedTime = DateTime.Now.AddHours((double)nextStep.TimeLimit);
            }

            await this.persistenceProvider.CreateTaskAsync(task);

            return task;
        }

        /// <summary>
        /// 删除某个实例下临时任务
        /// </summary>
        private async Task RemoveTemporaryTask(Guid instanceId)
        {
            var temporaryTask = await this.taskProvider.GetTemporaryTaskAsync(instanceId);

            if (temporaryTask.Count() > 0)
            {
                foreach (var task in temporaryTask)
                {
                    await this.persistenceProvider.RemoveTaskAsync(task);
                }
            }
        }

        /// <summary>
        /// 删除某个实例下某个步骤的临时任务
        /// </summary>
        private async Task RemoveTemporaryTask(Guid instanceId, Guid stepId)
        {
            var temporaryTask = await this.taskProvider.GetTemporaryTaskAsync(instanceId, stepId);

            if (temporaryTask.Count() > 0)
            {
                foreach (var task in temporaryTask)
                {
                    await this.persistenceProvider.RemoveTaskAsync(task);
                }
            }
        }

        /// <summary>
        /// 激活某个实例下某些步骤的临时任务
        /// </summary>
        private async Task ActivateTemporaryTask(Guid instanceId, WorkFlowTask nextStepTask, params Guid[] stepArray)
        {
            var tasks = await this.taskProvider.GetTemporaryTaskAsync(instanceId, stepArray);

            if (tasks.Count() > 0)
            {
                foreach (var task in tasks)
                {
                    task.PlannedTime = nextStepTask.PlannedTime;
                    task.ReceiveTime = nextStepTask.ReceiveTime;
                    task.SendTime = nextStepTask.ReceiveTime;
                    task.State = (int)WorkFlowTaskState.Pending;

                    await this.persistenceProvider.UpdateTaskAsync(task);
                }
            }
        }

        #endregion

        #region 回退任务相关规则判断

        /// <summary>
        /// 执行退回策略
        /// </summary>
        /// <param name="currentTask"></param>
        /// <param name="currentStep"></param>
        /// <param name="execution"></param>
        /// <returns></returns>
        private async Task<bool> ExecuteBackBackTactic(WorkFlowExecution execution)
        {
            bool isNeedWating = false;

            string note;

            //第一步退回，不判断策略
            if (execution.Task.IsStart())
            {
                await HandleUpdateWorkFlowTaskStateAsync(execution.Task, execution.Comment, execution.Step.IsNeedSignature(), WorkFlowTaskState.Returned);
            }
            else
            {
                //获取当前步骤分发的所有任务
                var taskList = await this.taskProvider.GetDistributionTaskAsync(execution.Task.InstanceId, execution.Task.Sort, execution.Step.StepId);

                if (execution.Step.IsBackTacticBy(WorkFlowBackTacticKinds.ReturnByTactic))
                {
                    //依据策略退回 
                    switch ((WorkFlowHandleTacticKinds)execution.Step.HandleTactic)
                    {
                        //所有人必须同意,如果一人不同意则全部退回
                        case WorkFlowHandleTacticKinds.AllAgree:

                            foreach (var task in taskList)
                            {
                                if (task.Id != execution.Task.Id)
                                {
                                    if (task.IsNotExecute())
                                    {
                                        note = $"{execution.Sender.Name}已选择退回，步骤必须全员同意才可以抵达至下一步";

                                        await HandleUpdateWorkFlowTaskStateAsync(task, "", false, WorkFlowTaskState.ReturnedByOthers, note);
                                    }
                                }
                                else
                                {
                                    await HandleUpdateWorkFlowTaskStateAsync(task, execution.Comment, execution.Step.IsNeedSignature(), WorkFlowTaskState.Returned);
                                }
                            }

                            break;

                        // 一人同意即可，退回自己的，不影响其他人
                        case WorkFlowHandleTacticKinds.OneAgree:

                            if (taskList.Count > 1)
                            {
                                var noReturned = taskList.Where(t => t.IsNot(WorkFlowTaskState.Returned));
                                if (noReturned.Count() - 1 > 0)
                                {
                                    isNeedWating = true;
                                }
                            }

                            note = isNeedWating ? "步骤已退回，但还需等待其他人处理" : "";

                            await HandleUpdateWorkFlowTaskStateAsync(execution.Task, execution.Comment, execution.Step.IsNeedSignature(), WorkFlowTaskState.Returned, note);

                            break;

                        //依据人数比例，投票比例大于规定比例,则退回全部，如果小于则等待，其他人继续投票
                        case WorkFlowHandleTacticKinds.PercentageAgree:

                            if (taskList.Count > 1)
                            {
                                decimal percentage = 100 - (execution.Step.Percentage <= 0 ? 100 : execution.Step.Percentage);//比例

                                decimal backPercentage = Math.Round(((decimal)(taskList.Where(t => t.Is(WorkFlowTaskState.Returned)).Count() + 1) / (decimal)taskList.Count) * 100);

                                if (backPercentage < percentage)
                                {
                                    isNeedWating = true;
                                }
                                else
                                {
                                    foreach (var task in taskList)
                                    {
                                        if (task.Id != execution.Task.Id && task.IsNotExecute())
                                        {
                                            note = $"有{backPercentage}%的人选择退回当前步骤，高于设定比例{percentage}%，由系统判定退回当前步骤";

                                            await HandleUpdateWorkFlowTaskStateAsync(task, "", false, WorkFlowTaskState.ReturnedByOthers, note);
                                        }
                                    }
                                }
                            }

                            note = isNeedWating ? "步骤已退回，但还需等待其他人处理" : "";

                            await HandleUpdateWorkFlowTaskStateAsync(execution.Task, execution.Comment, execution.Step.IsNeedSignature(), WorkFlowTaskState.Returned, note);

                            break;

                        //独立处理
                        case WorkFlowHandleTacticKinds.Independent:

                            await HandleUpdateWorkFlowTaskStateAsync(execution.Task, execution.Comment, execution.Step.IsNeedSignature(), WorkFlowTaskState.Returned);

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

                        if (task.Id == execution.Task.Id)
                        {
                            await HandleUpdateWorkFlowTaskStateAsync(task, execution.Comment, execution.Step.IsNeedSignature(), WorkFlowTaskState.Returned);
                        }
                        else
                        {
                            note = $"当前步骤已被{execution.Sender.Name}退回";

                            await HandleUpdateWorkFlowTaskStateAsync(task, "", false, WorkFlowTaskState.Returned, note);
                        }
                    }
                }

                //退回时要退回其它步骤发来的同级任务
                foreach (var task in taskList)
                {
                    if (!execution.ToStepCollection.ContainsKey(task.PrevStepId))
                    {
                        execution.ToStepCollection.Add(task.PrevStepId, new List<WorkFlowUser>());
                    }
                }

                //如果当前步骤是会签步骤，则退回重新会签
                if (execution.Step.IsNeedCountersignature())
                {
                    var countersignatureStepPrevSteps = execution.Definition.GetPrevSteps(execution.Step.StepId);

                    foreach (var prevStep in countersignatureStepPrevSteps)
                    {
                        if (!execution.ToStepCollection.ContainsKey(prevStep.StepId))
                        {
                            execution.ToStepCollection.Add(prevStep.StepId, new List<WorkFlowUser>());
                        }
                    }
                }
            }

            return isNeedWating;
        }

        #endregion

        #region 回退任务

        /// <summary>
        /// 创建退回步骤
        /// </summary>
        /// <param name="currentTask"></param>
        /// <param name="currentStep"></param>
        /// <param name="execution"></param>
        /// <returns></returns>
        private async Task<List<WorkFlowTask>> CreateBackStepTask(WorkFlowExecution execution)
        {
            // 备份回退步骤分发的全部任务
            var sourceBackStepTasks = await HandleBackupAllBackStepTask(execution);

            // 回退会签
            bool isBack = await HandleCountersignatureStepBack(execution);

            // 回退子流程
            await HandleSubFlowStepBack(execution);

            // 回退临时任务
            await HandleTemporaryTaskBack(execution);

            var backStepTasks = new List<WorkFlowTask>();

            if (isBack && sourceBackStepTasks.Count > 0)
            {
                foreach (var sourceTask in sourceBackStepTasks)
                {
                    var createdBackStepTask = await HandleCreateBackStepTask(sourceTask, execution);

                    backStepTasks.Add(createdBackStepTask);
                }
            }

            return backStepTasks;
        }

        /// <summary>
        /// 实施创建回退任务
        /// </summary>
        /// <param name="sourceTask"></param>
        /// <param name="execution"></param>
        /// <returns></returns>
        private async Task<WorkFlowTask> HandleCreateBackStepTask(WorkFlowTask sourceTask, WorkFlowExecution execution)
        {
            WorkFlowTask task = new WorkFlowTask();

            task.FlowId = sourceTask.FlowId;
            task.FlowName = sourceTask.FlowName;
            task.GroupId = sourceTask.GroupId;
            task.Type = (int)WorkFlowTaskKinds.Return;
            task.InstanceId = sourceTask.InstanceId;
            task.Note = $"由步骤【{execution.Task.StepName}】执行退回所产生的任务";
            task.PrevTaskId = execution.Task.Id;
            task.PrevStepId = sourceTask.PrevStepId;
            task.ReceiverId = sourceTask.ReceiverId;
            task.ReceiverName = sourceTask.ReceiverName;
            task.ReceiveTime = DateTime.Now;
            task.SenderId = execution.Task.ReceiverId;
            task.SenderName = execution.Task.ReceiverName;
            task.SendTime = DateTime.Now;
            task.State = (int)WorkFlowTaskState.Pending;
            task.StepId = sourceTask.StepId;
            task.StepName = sourceTask.StepName;
            task.Sort = execution.Task.Sort + 1;
            task.Title = sourceTask.Title;
            task.CompanyId = sourceTask.CompanyId;

            if (execution.Step.TimeLimit > 0)
            {
                task.PlannedTime = DateTime.Now.AddHours((double)execution.Step.TimeLimit);
            }

            await this.persistenceProvider.CreateTaskAsync(task);

            return task;
        }

        /// <summary>
        /// 备份全部的回退步骤所分发的任务
        /// </summary>
        /// <param name="execution"></param>
        /// <returns></returns>
        private async Task<List<WorkFlowTask>> HandleBackupAllBackStepTask(WorkFlowExecution execution)
        {
            var tasks = new List<WorkFlowTask>();

            foreach (var step in execution.ToStepCollection)
            {
                //获取当前步骤最后一次分发的任务
                var backStepTask = await this.taskProvider.GetNewestDistributionTaskAsync(execution.Task.InstanceId, step.Key);

                tasks.AddRange(backStepTask);
            }

            return tasks.Distinct(new WorkFlowReceiverTaskComparer()).ToList();
        }

        /// <summary>
        /// 处理会签回退
        /// </summary>
        /// <returns></returns>
        private async Task<bool> HandleCountersignatureStepBack(WorkFlowExecution execution)
        {
            // 当前步骤的下一步是否是会签步骤，处理会签形式的退回

            var countersignatureStep = execution.Definition.GetNextSteps(execution.Step.StepId).Find(t => t.IsNeedCountersignature());

            bool isCountersignature = countersignatureStep != null;

            bool isBack = true;

            if (isCountersignature)
            {
                isBack = await IsCountersignatureStepBacked(countersignatureStep, execution);

                if (isBack)
                {
                    var sameLevelStepTaskList = await HandleGetSameLevelStepNotExecuteTask(execution);

                    foreach (var task in sameLevelStepTaskList)
                    {
                        if (task.Id == execution.Task.Id || task.IsExecute())
                        {
                            continue;
                        }

                        await HandleUpdateWorkFlowTaskStateAsync(task, "", false, WorkFlowTaskState.ReturnedByOthers, "会签已结束，此步骤因其他人员处理产生回退");
                    }
                }
            }

            return isBack;
        }

        /// <summary>
        /// 处理子流程回退
        /// </summary>
        /// <param name="execution"></param>
        /// <returns></returns>
        private async Task HandleSubFlowStepBack(WorkFlowExecution execution)
        {
            //当前步骤是子流程步骤，则要作废子流程实例
            if (execution.Step.IsSubFlowStep() && !string.IsNullOrEmpty(execution.Step.SubFlowId))
            {
                await this.persistenceProvider.RemoveInstanceAsync(execution.Task.SubFlowInstanceId);
            }
        }

        /// <summary>
        /// 处理临时任务回退
        /// </summary>
        /// <param name="execution"></param>
        /// <returns></returns>
        private async Task HandleTemporaryTaskBack(WorkFlowExecution execution)
        {
            // 删除临时任务
            var countersignatureStep = execution.Definition.GetNextSteps(execution.Step.StepId).Find(t => t.IsNeedCountersignature());

            bool isCountersignature = countersignatureStep != null;

            // 删除临时任务
            if (isCountersignature)
            {
                await RemoveTemporaryTask(execution.Instance.Id);
            }
            else
            {
                await RemoveTemporaryTask(execution.Instance.Id, execution.Step.StepId);
            }
        }

        #endregion

        #region 更新任务状态

        /// <summary>
        /// 处理更新任务状态，默认是完成状态
        /// </summary>
        /// <param name="task"></param>
        /// <param name="comment"></param>
        /// <param name="isNeedSignature"></param>
        /// <param name="state"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        private async Task HandleUpdateWorkFlowTaskStateAsync(WorkFlowTask task, string comment, bool isNeedSignature, WorkFlowTaskState state, string note = "")
        {
            task.Comment = comment;
            task.IsNeedSign = isNeedSignature;
            task.State = (int)state;
            task.Note = note;
            task.ExecutedTime = DateTime.Now;

            await this.persistenceProvider.UpdateTaskAsync(task);
        }

        #endregion

        #region 判断步骤是通过还是回退

        /// <summary>
        /// 查询一个用户在一个步骤是否有未处理任务
        /// </summary>
        /// <returns></returns>
        private async Task<bool> IsHasNotExecuteTask(Guid instanceId, Guid stepId, Guid userId)
        {
            return await this.taskProvider.IsTheReceiverHasNotExecuteTask(instanceId, userId, stepId);
        }

        /// <summary>
        /// 如果当前步骤是子流程步骤，并且策略是【子流程完成后才能提交】,则要判断子流程是否已完成
        /// </summary>
        /// <returns></returns>
        private async Task<bool> IsSubFlowStepPassed(WorkFlowExecution execution)
        {
            bool isPass = true;

            if (execution.Step.IsSubFlowStep() && !string.IsNullOrEmpty(execution.Step.SubFlowId) && !execution.Task.SubFlowInstanceId.Equals(default))
            {
                if (execution.Step.IsExecuteAfterSubFlowEnd())
                {
                    var subFlowInstance = await HandleGetWorkFlowInstanceAsync(execution.Task.SubFlowInstanceId);

                    switch ((WorkFlowSubFlowTacticKinds)execution.Step.SubFlowTactic)
                    {
                        case WorkFlowSubFlowTacticKinds.SubFlowCompleted:

                            isPass = subFlowInstance.State == (int)WorkFlowInstanceState.Completed;

                            break;
                        case WorkFlowSubFlowTacticKinds.SubFlowFinished:

                            isPass = subFlowInstance.State != (int)WorkFlowInstanceState.Approving;

                            break;
                    }
                }

                // 待定，更严格的检查，查找是否还有没执行的任务
                //isPass = !execution.GroupTaskCollection.Any(t => t.InstanceId == execution.Task.SubFlowInstanceId
                //                                            && t.GroupId == execution.Task.GroupId
                //                                            && t.FlowId == Guid.Parse(execution.Step.SubFlowId)
                //                                            && t.IsNotExecute());

                return isPass;
            }

            return isPass;
        }

        /// <summary>
        /// 会签步骤是否可以完成
        /// </summary>
        /// <returns></returns>
        private async Task<bool> IsCountersignatureStepPassed(WorkFlowStep countersignatureStep, WorkFlowExecution execution)
        {
            //获取会签步骤的所有上一步
            var prevSteps = execution.Definition.GetPrevSteps(countersignatureStep.StepId);

            var isPass = true;

            switch ((WorkFlowCountersignatureTacticKinds)countersignatureStep.CountersignatureTactic)
            {
                case WorkFlowCountersignatureTacticKinds.AllAgree://所有步骤同意

                    isPass = true;

                    foreach (var prevStep in prevSteps)
                    {
                        if (!await IsStepPassed(prevStep, execution.Task.InstanceId, execution.Task.Sort))
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
                        if (await IsStepPassed(prevStep, execution.Task.InstanceId, execution.Task.Sort))
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
                        if (await IsStepPassed(prevStep, execution.Task.InstanceId, execution.Task.Sort))
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
        private async Task<bool> IsStepPassed(WorkFlowStep step, Guid instanceId, int sort)
        {
            var tasks = await this.taskProvider.GetDistributionTaskAsync(instanceId, sort, step.StepId);

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
        private async Task<bool> IsCountersignatureStepBacked(WorkFlowStep countersignatureStep, WorkFlowExecution execution)
        {
            //获取会签步骤的所有上一步
            var prevSteps = execution.Definition.GetPrevSteps(countersignatureStep.StepId);

            var isBack = true;

            switch ((WorkFlowCountersignatureTacticKinds)countersignatureStep.CountersignatureTactic)
            {
                //所有步骤处理，如果一个步骤退回则退回
                case WorkFlowCountersignatureTacticKinds.AllAgree:

                    isBack = false;

                    foreach (var step in prevSteps)
                    {
                        if (await IsStepBacked(step, execution.Task.InstanceId, execution.Task.Sort))
                        {
                            isBack = true;
                            break;
                        }
                    }

                    break;

                //一个步骤退回,如果有一个步骤同意，则不退回
                case WorkFlowCountersignatureTacticKinds.OneAgree:

                    foreach (var step in prevSteps)
                    {
                        if (!await IsStepBacked(step, execution.Task.InstanceId, execution.Task.Sort))
                        {
                            isBack = false;
                            break;
                        }
                    }

                    break;

                //依据比例退回
                case WorkFlowCountersignatureTacticKinds.PercentageAgree:

                    int backCount = 0;

                    foreach (var step in prevSteps)
                    {
                        if (await IsStepBacked(step, execution.Task.InstanceId, execution.Task.Sort))
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
        private async Task<bool> IsStepBacked(WorkFlowStep step, Guid instanceId, int sort)
        {
            var tasks = await this.taskProvider.GetDistributionTaskAsync(instanceId, sort, step.StepId);

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

        #region 拦截器执行

        private WorkFlowInterceptorResults ExecuteInterceptor(string interceptorAssembly, WorkFlowTask task)
        {
            if (!string.IsNullOrWhiteSpace(interceptorAssembly))
            {
                var method = interceptorAssembly.Trim();

                var args = new WorkFlowInterceptorArgs(task);

                WorkFlowConstant.ExecuteMethod(method, args, out WorkFlowInterceptorResults result);

                if (!result.IsPassed)
                {
                    throw new InvalidOperationException(result.Message);
                }

                return result;
            }

            return null;
        }

        #endregion

        #region 发送消息

        public void SendMessage(string title, string message, params Guid[] receiver)
        {
            title = $"【审批】{title}";
            message = $"【审批】{message}";

            this.noticeSender.Push(title, message, receiver);
        }

        #endregion

        #region 获取任务

        /// <summary>
        /// 获取当前任务的所有同级步骤没有处理过的任务
        /// </summary>
        private async Task<List<WorkFlowTask>> HandleGetSameLevelStepNotExecuteTask(WorkFlowExecution execution)
        {
            var sameLevelSteps = execution.Definition.GetSameLevelSteps(execution.Step.StepId);

            var stepArray = sameLevelSteps.Select(t => t.StepId).ToArray();

            return await this.taskProvider.GetNotExecuteDistributionTaskAsync(execution.Task.InstanceId, stepArray);
        }

        #endregion

        public async Task<Dictionary<int, List<WorkFlowStep>>> GetWorkFlowProcessAsync(WorkFlowInstance instance)
        {
            var result = new Dictionary<int, List<WorkFlowStep>>
            {
                { 0, new List<WorkFlowStep>() }, //已初始化
                { 1, new List<WorkFlowStep>() }, //等待中
                { 2, new List<WorkFlowStep>() }, //已挂起
                { 3, new List<WorkFlowStep>() } //已完成
            };

            var tasks = await this.persistenceProvider.GetAllTaskAsync(instance.Id);

            if (tasks.Any())
            {
                var groupId = tasks.First().GroupId;

                var definition = WorkFlowDefinition.Parse(instance.FlowRuntimeJson);

                foreach (var step in definition.Steps)
                {
                    var stepTasks = tasks.FindAll(t => t.StepId == step.StepId);

                    if (stepTasks.Count() > 0)
                    {
                        int sort = stepTasks.Max(t => t.Sort);

                        if (await IsStepPassed(step, instance.Id, sort))
                        {
                            result[3].Add(step);
                        }
                        else if (await IsStepBacked(step, instance.Id, sort))
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
