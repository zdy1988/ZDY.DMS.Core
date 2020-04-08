using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore.Bootstrapper.Service;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.WorkFlowService.Models;
using ZDY.DMS.Services.WorkFlowService.ServiceContracts;

namespace ZDY.DMS.Services.WorkFlowService.Implementation
{
    public class WorkFlowInstanceService : ServiceBase<WorkFlowServiceModule>, IWorkFlowInstanceService
    {
        private readonly IRepository<Guid, WorkFlowInstance> workFlowInstanceRepository;

        public WorkFlowInstanceService(Func<Type, IRepositoryContext> repositoryContextFactory)
            : base(repositoryContextFactory)
        {
            this.workFlowInstanceRepository = this.GetRepository<Guid, WorkFlowInstance>();
        }

        public async Task<WorkFlowInstance> GetWorkFlowInstanceByKeyAsync(Guid instanceId)
        {
            return await this.workFlowInstanceRepository.FindAsync(t => t.Id.Equals(instanceId) && t.IsDisabled.Equals(false));
        }
    }
}
