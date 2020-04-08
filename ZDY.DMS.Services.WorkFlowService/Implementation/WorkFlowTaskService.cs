using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore.Bootstrapper.Service;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.WorkFlowService.Enums;
using ZDY.DMS.Services.WorkFlowService.Models;
using ZDY.DMS.Services.WorkFlowService.ServiceContracts;

namespace ZDY.DMS.Services.WorkFlowService.Implementation
{
    public class WorkFlowTaskService : ServiceBase<WorkFlowServiceModule>, IWorkFlowTaskService
    {
        private readonly IRepository<Guid, WorkFlowTask> workFlowTaskRepository;

        public WorkFlowTaskService(Func<Type, IRepositoryContext> repositoryContextFactory)
            : base(repositoryContextFactory)
        {
            this.workFlowTaskRepository = this.GetRepository<Guid, WorkFlowTask>();
        }

        public async Task<WorkFlowTask> GetWorkFlowTaskByKeyAsync(Guid taskId)
        { 
            return await this.workFlowTaskRepository.FindByKeyAsync(taskId);
        }

        public async Task WorkFlowTaskOpenedAsync(Guid taskId)
        {
            var task = await this.workFlowTaskRepository.FindByKeyAsync(taskId);

            task.State = (int)WorkFlowTaskState.Opened;
            task.OpenedTime = DateTime.Now;

            await this.workFlowTaskRepository.UpdateAsync(task);
            await this.RepositoryContext.CommitAsync();
        }
    }
}
