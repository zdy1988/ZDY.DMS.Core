using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZDY.DMS.Services.WorkFlowService.Models;

namespace ZDY.DMS.Services.WorkFlowService.ServiceContracts
{
    public interface IWorkFlowFormService
    {
        Task<IEnumerable<WorkFlowForm>> GetPublishedWorkFlowForms(Guid companyID);

        Task<IEnumerable<WorkFlowForm>> GetDesigningWorkFlowForms(Guid companyID);
    }
}
