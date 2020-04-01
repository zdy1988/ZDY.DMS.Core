using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZDY.DMS.Services.WorkFlowService.Models;

namespace ZDY.DMS.Services.WorkFlowService.ServiceContracts
{
    public interface IWorkFlowFormService
    {
        Task<IEnumerable<WorkFlowForm>> GetPublishedWorkFlowFormCollectionAsync(Guid companyID);

        Task<IEnumerable<WorkFlowForm>> GetDesigningWorkFlowFormCollectionAsync(Guid companyID);

        Task<WorkFlowForm> GetPublishedWorkFlowFormByKeyAsync(Guid formId);
    }
}
