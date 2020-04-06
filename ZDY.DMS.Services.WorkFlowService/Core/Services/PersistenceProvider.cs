using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.WorkFlowService.Core.Extensions;
using ZDY.DMS.Services.WorkFlowService.Core.Interfaces;
using ZDY.DMS.Services.WorkFlowService.Enums;
using ZDY.DMS.Services.WorkFlowService.Models;

namespace ZDY.DMS.Services.WorkFlowService.Core.Services
{
    public class PersistenceProvider : IPersistenceProvider
    {
        private readonly IRepositoryContext repositoryContext;
        private readonly IRepository<Guid, WorkFlow> workFlowRepository;
        private readonly IRepository<Guid, WorkFlowForm> workFlowFormRepository;
        private readonly IRepository<Guid, WorkFlowTask> workFlowTaskRepository;
        private readonly IRepository<Guid, WorkFlowInstance> workFlowInstanceRepository;

        public PersistenceProvider(Func<Type, IRepositoryContext> repositoryContextFactory)
        {
            this.repositoryContext = repositoryContextFactory.Invoke(typeof(WorkFlowServiceModule));

            this.workFlowRepository = this.repositoryContext.GetRepository<Guid, WorkFlow>();
            this.workFlowFormRepository = this.repositoryContext.GetRepository<Guid, WorkFlowForm>();
            this.workFlowTaskRepository = this.repositoryContext.GetRepository<Guid, WorkFlowTask>();
            this.workFlowInstanceRepository = this.repositoryContext.GetRepository<Guid, WorkFlowInstance>();
        }

        /// <summary>
        /// 获取一个流程数据
        /// </summary>
        /// <param name="flowId"></param>
        /// <returns></returns>
        public async Task<WorkFlow> GetWorkFlowAsync(Guid flowId)
        {
            return await workFlowRepository.FindAsync(t => t.Id == flowId && t.IsDisabled == false);
        }

        /// <summary>
        /// 获取一个表单数据
        /// </summary>
        /// <returns></returns>
        public async Task<WorkFlowForm> GetWorkFlowFormAsync(Guid formId)
        {
            return await workFlowFormRepository.FindAsync(t => t.Id == formId && t.IsDisabled == false);
        }

        /// <summary>
        /// 获取一个流程实例
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        public async Task<WorkFlowInstance> GetWorkFlowInstanceAsync(Guid instanceId)
        {
            return await workFlowInstanceRepository.FindAsync(t => t.Id == instanceId && t.IsDisabled == false);
        }

        /// <summary>
        /// 创建一个流程实例
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public async Task CreateInstanceAsync(WorkFlowInstance instance)
        {
            await workFlowInstanceRepository.AddAsync(instance);
        }

        /// <summary>
        /// 删除一个流程实例
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        public async Task RemoveInstanceAsync(Guid instanceId)
        {
            //删除实例
            var instance = await workFlowInstanceRepository.FindByKeyAsync(instanceId);

            if (instance!=null)
            {
                instance.IsDisabled = true;

                await workFlowInstanceRepository.UpdateAsync(instance);

                //删除任务
                var tasks = await workFlowTaskRepository.FindAllAsync(t => t.InstanceId == instanceId);

                if (tasks.Count() > 0)
                {
                    foreach (var task in tasks)
                    {
                        task.IsDisabled = true;

                        await workFlowTaskRepository.UpdateAsync(task);
                    }
                }
            }
        }

        /// <summary>
        /// 更新一个流程实例
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public async Task UpdateInstanceAsync(WorkFlowInstance instance)
        {
            await workFlowInstanceRepository.UpdateAsync(instance);
        }

        /// <summary>
        /// 获取一个流程任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public async Task<WorkFlowTask> GetWorkFlowTaskAsync(Guid taskId)
        {
            return await workFlowTaskRepository.FindAsync(t => t.Id == taskId && t.IsDisabled == false);
        }

        /// <summary>
        /// 获取发布子流程的根流程任务
        /// </summary>
        /// <param name="instanceId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<WorkFlowTask> GetRootInstanceTaskAsync(Guid instanceId, Guid groupId)
        {
            var parentTask = await workFlowTaskRepository.FindAsync(t => t.GroupId == groupId
                                                                      && t.SubFlowInstanceId == instanceId
                                                                      && t.State != (int)WorkFlowTaskKinds.Copy
                                                                      && t.IsDisabled == false);

            return parentTask;
        }

        /// <summary>
        /// 新建一个任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public async Task CreateTaskAsync(WorkFlowTask task)
        {
            await this.workFlowTaskRepository.AddAsync(task);
        }

        /// <summary>
        /// 删除一个任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public async Task RemoveTaskAsync(WorkFlowTask task)
        {
            await this.workFlowTaskRepository.RemoveAsync(task);
        }

        /// <summary>
        /// 更新一个任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public async Task UpdateTaskAsync(WorkFlowTask task)
        {
            await workFlowTaskRepository.UpdateAsync(task);
        }

        /// <summary>
        /// 获取某个步骤所有分发的任务
        /// </summary>
        /// <returns></returns>
        public async Task<List<WorkFlowTask>> GetAllStepDistributionTaskAsync(Guid stepId, Guid flowId, Guid instanceId, Guid groupId, int sort)
        {
            var list = await workFlowTaskRepository.FindAllAsync(t => t.FlowId == flowId
                                                                   && t.InstanceId == instanceId
                                                                   && t.StepId == stepId
                                                                   && t.GroupId == groupId
                                                                   && t.Sort == sort
                                                                   && t.State != (int)WorkFlowTaskKinds.Copy
                                                                   && t.IsDisabled == false);

            return list.ToList();
        }

        /// <summary>
        /// 获取某个步骤最新分发的任务
        /// </summary>
        /// <param name="stepId"></param>
        /// <param name="flowId"></param>
        /// <param name="instanceId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<List<WorkFlowTask>> GetAllStepDistributionNewestTaskAsync(Guid stepId, Guid flowId, Guid instanceId, Guid groupId)
        {
            var list = await workFlowTaskRepository.FindAllAsync(t => t.FlowId == flowId
                                                                   && t.InstanceId == instanceId
                                                                   && t.StepId == stepId
                                                                   && t.GroupId == groupId
                                                                   && t.State != (int)WorkFlowTaskKinds.Copy
                                                                   && t.IsDisabled == false);

            var maxSort = list.Select(t => t.Sort).Max();

            return list.Where(t => t.Sort == maxSort).ToList();
        }

        /// <summary>
        /// 获取某些步骤没有处理过的任务
        /// </summary>
        /// <param name="currentTask"></param>
        /// <returns></returns>
        public async Task<List<WorkFlowTask>> GetAllStepDistributionNotExecuteTaskAsync(Guid[] stepArray, Guid flowId, Guid instanceId, Guid groupId)
        {
            var list = await workFlowTaskRepository.FindAllAsync(t => stepArray.Contains(t.StepId)
                                                                   && t.InstanceId == instanceId
                                                                   && t.GroupId == groupId
                                                                   && t.FlowId == flowId
                                                                   && t.State != (int)WorkFlowTaskState.Handled
                                                                   && t.State != (int)WorkFlowTaskState.Returned
                                                                   && t.State != (int)WorkFlowTaskState.HandledByOthers
                                                                   && t.State != (int)WorkFlowTaskState.ReturnedByOthers
                                                                   && t.State != (int)WorkFlowTaskKinds.Copy
                                                                   && t.IsDisabled == false);

            return list.ToList();
        }

        /// <summary>
        /// 判断某个用户是否在某个步骤还有未处理的任务
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsTheReceiverHasNotExecuteTask(Guid flowId, Guid instanceId, Guid stepId, Guid groupId, Guid userId)
        {
            var count = await workFlowTaskRepository.CountAsync(t => t.FlowId == flowId
                                                                  && t.InstanceId == instanceId
                                                                  && t.StepId == stepId
                                                                  && t.GroupId == groupId
                                                                  && t.ReceiverId == userId
                                                                  && t.State != (int)WorkFlowTaskState.Handled
                                                                  && t.State != (int)WorkFlowTaskState.Returned
                                                                  && t.State != (int)WorkFlowTaskState.HandledByOthers
                                                                  && t.State != (int)WorkFlowTaskState.ReturnedByOthers
                                                                  && t.IsDisabled == false);

            return count > 0;
        }

        /// <summary>
        /// 获取某个实例下所有的临时任务
        /// </summary>
        /// <returns></returns>
        public async Task<List<WorkFlowTask>> GetAllTemporaryTaskAsync(Guid instanceId)
        {
            var list = await workFlowTaskRepository.FindAllAsync(t => t.InstanceId == instanceId 
                                                                   && t.State == (int)WorkFlowTaskState.Waiting
                                                                   && t.IsDisabled == false);
            return list.ToList();
        }

        /// <summary>
        /// 获取某个实例下规定步骤范围内的临时任务
        /// </summary>
        /// <param name="instanceId"></param>
        /// <param name="stepArray"></param>
        /// <returns></returns>
        public async Task<List<WorkFlowTask>> GetAllTemporaryTaskAsync(Guid instanceId, params Guid[] stepArray)
        {
            var list = await workFlowTaskRepository.FindAllAsync(t => t.InstanceId == instanceId
                                                                   && stepArray.Contains(t.StepId)
                                                                   && t.State == (int)WorkFlowTaskState.Waiting
                                                                   && t.IsDisabled == false);
            return list.ToList();
        }

        /// <summary>
        /// 获取一个实例中的所有任务
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        public async Task<List<WorkFlowTask>> GetAllTaskAsync(Guid instanceId)
        {
            var list = await this.workFlowTaskRepository.FindAllAsync(t => t.InstanceId == instanceId
                                                                        && t.IsNot(WorkFlowTaskKinds.Copy)
                                                                        && t.IsDisabled == false);

            return list.ToList();
        }
    }
}
