using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.WorkFlowService.Core.Models;
using ZDY.DMS.Services.WorkFlowService.ServiceContracts;
using ZDY.DMS.Services.WorkFlowService.Models;
using ZDY.DMS.Tools;
using ZDY.DMS.Services.WorkFlowService.Enums;
using ZDY.DMS.AspNetCore.Bootstrapper.Service;
using ZDY.DMS.Services.WorkFlowService.Core.Interfaces;

namespace ZDY.DMS.Services.WorkFlowService.Implementation
{
    public class WorkFlowHostService : ServiceBase<WorkFlowServiceModule>, IWorkFlowHostService
    {
        private readonly IWorkFlowExecutor workFlowExecutor;

        private readonly IRepository<Guid, WorkFlowTask> workFlowTaskRepository;

        public WorkFlowHostService(IWorkFlowExecutor workFlowExecutor) 
        {
            this.workFlowExecutor = workFlowExecutor;

            this.workFlowTaskRepository = RepositoryContext.GetRepository<Guid, WorkFlowTask>();
        }

        /// <summary>
        /// 创建一个流程实例，即发起一个流程
        /// </summary>
        /// <param name="instance"></param>
        public async Task StartUp(WorkFlowInstance instance)
        {
            await this.workFlowExecutor.ExecuteStartAsync(instance, Guid.NewGuid());
        }

        /// <summary>
        /// 处理流程
        /// </summary>
        /// <param name="execution">处理实体</param>
        /// <returns></returns>
        public async Task Execute(WorkFlowExecute execute)
        {
            await this.workFlowExecutor.ExecuteAsync(execute);
        }

        /// <summary>
        /// 获取一个实例中包含评论的任务
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public async Task<List<WorkFlowTask>> GetWorkFlowCommentTaskListAsync(Guid instanceId)
        {
            var taskList = await this.workFlowTaskRepository.FindAllAsync(t => t.InstanceId == instanceId
                                                                            && !string.IsNullOrEmpty(t.Comment)
                                                                            && t.State != (int)WorkFlowTaskState.Waiting
                                                                            && t.IsDisabled == false,
            query => query.Desc(o => o.ExecutedTime));

            return taskList.ToList();
        }

        /// <summary>
        /// 获取一个实例中包含的所有任务
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public async Task<List<WorkFlowTask>> GetWorkFlowProcessTaskListAsync(Guid instanceId)
        {
            var taskList = await workFlowTaskRepository.FindAllAsync(t => t.InstanceId == instanceId
                                                                       && t.State != (int)WorkFlowTaskState.Waiting
                                                                       && t.IsDisabled == false,
            query => query.Desc(a => a.ExecutedTime));

            return taskList.ToList();
        }

        /// <summary>
        /// 获取一个实例中任务的
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public async Task<Dictionary<int, List<WorkFlowStep>>> GetWorkFlowProcessAsync(WorkFlowInstance instance)
        {
            return await this.workFlowExecutor.GetWorkFlowProcessAsync(instance);
        }
    }
}
