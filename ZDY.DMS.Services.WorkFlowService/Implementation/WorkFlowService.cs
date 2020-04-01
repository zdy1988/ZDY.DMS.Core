using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore.Service;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.WorkFlowService.Enums;
using ZDY.DMS.Services.WorkFlowService.ServiceContracts;

namespace ZDY.DMS.Services.WorkFlowService.Implementation
{
    public class WorkFlowService : ServiceBase<WorkFlowServiceModule>, IWorkFlowService
    {
        private readonly IRepository<Guid, Models.WorkFlow> workFlowRepository;

        public WorkFlowService(Func<Type, IRepositoryContext> repositoryContextFactory)
            : base(repositoryContextFactory)
        {
            this.workFlowRepository = this.GetRepository<Guid, Models.WorkFlow>();
        }

        private async Task<IEnumerable<Models.WorkFlow>> GetWorkFlows(Guid companyID, WorkFlowState workFlowState)
        {
            var list = await this.workFlowRepository
                .FindAllAsync(t => t.CompanyId == companyID
                                && t.State == (int)workFlowState
                                && t.IsDisabled == false);
            return list;
        }

        public async Task<IEnumerable<Models.WorkFlow>> GetInstalledWorkFlows(Guid companyID)
        {
            return await GetWorkFlows(companyID, WorkFlowState.Installed);
        }

        public async Task<IEnumerable<Models.WorkFlow>> GetDesigningWorkFlows(Guid companyID)
        {
            return await GetWorkFlows(companyID, WorkFlowState.Designing);
        }
    }
}
