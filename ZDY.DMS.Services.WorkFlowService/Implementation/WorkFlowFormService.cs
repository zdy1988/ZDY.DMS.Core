using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore.Service;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.WorkFlowService.Enums;
using ZDY.DMS.Services.WorkFlowService.Models;
using ZDY.DMS.Services.WorkFlowService.ServiceContracts;

namespace ZDY.DMS.Services.WorkFlowService.Implementation
{
    public class WorkFlowFormService : ServiceBase<WorkFlowServiceModule>, IWorkFlowFormService
    {
        private readonly IRepository<Guid, WorkFlowForm> workFlowFormRepository;

        public WorkFlowFormService(Func<Type, IRepositoryContext> repositoryContextFactory)
            : base(repositoryContextFactory)
        {
            this.workFlowFormRepository = this.GetRepository<Guid, WorkFlowForm>();
        }

        private async Task<IEnumerable<WorkFlowForm>> GetWorkFlowFormCollectionAsync(Guid companyID, WorkFlowFormState workFlowFormState)
        {
            var list = await this.workFlowFormRepository
                .FindAllAsync(t => t.CompanyId == companyID
                                && t.State == (int)workFlowFormState
                                && t.IsDisabled == false);
            return list;
        }

        public async Task<IEnumerable<WorkFlowForm>> GetPublishedWorkFlowFormCollectionAsync(Guid companyId)
        {
            return await GetWorkFlowFormCollectionAsync(companyId, WorkFlowFormState.Published);
        }

        public async Task<IEnumerable<WorkFlowForm>> GetDesigningWorkFlowFormCollectionAsync(Guid companyId)
        {
            return await GetWorkFlowFormCollectionAsync(companyId, WorkFlowFormState.Designing);
        }

        public async Task<WorkFlowForm> GetPublishedWorkFlowFormByKeyAsync(Guid formId)
        { 
            return await this.workFlowFormRepository.FindAsync(t => t.Id == formId && t.State == (int)WorkFlowFormState.Published);
        }
    }
}
