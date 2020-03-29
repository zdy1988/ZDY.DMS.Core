using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.WorkFlowService.Enums;
using ZDY.DMS.Services.WorkFlowService.Models;
using ZDY.DMS.Services.WorkFlowService.ServiceContracts;

namespace ZDY.DMS.Services.WorkFlowService.Implementation
{
    public class WorkFlowFormService : IWorkFlowFormService
    {
        private readonly IRepositoryContext repositoryContext;
        private readonly IRepository<Guid, WorkFlowForm> workFlowFormRepository;

        public WorkFlowFormService(IRepositoryContext repositoryContext)
        {
            this.repositoryContext = repositoryContext;
            this.workFlowFormRepository = this.repositoryContext.GetRepository<Guid, WorkFlowForm>();
        }

        private async Task<IEnumerable<WorkFlowForm>> GetWorkFlowForms(Guid companyID, WorkFlowFormState workFlowFormState)
        {
            var list = await this.workFlowFormRepository
                .FindAllAsync(t => t.CompanyId == companyID
                                && t.State == (int)workFlowFormState
                                && t.IsDisabled == false);
            return list;
        }

        public async Task<IEnumerable<WorkFlowForm>> GetPublishedWorkFlowForms(Guid companyID)
        {
            return await GetWorkFlowForms(companyID, WorkFlowFormState.Published);
        }

        public async Task<IEnumerable<WorkFlowForm>> GetDesigningWorkFlowForms(Guid companyID)
        {
            return await GetWorkFlowForms(companyID, WorkFlowFormState.Designing);
        }
    }
}
