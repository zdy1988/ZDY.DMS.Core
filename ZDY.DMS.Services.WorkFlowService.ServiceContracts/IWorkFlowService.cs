using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZDY.DMS.Services.WorkFlowService.Models;

namespace ZDY.DMS.Services.WorkFlowService.ServiceContracts
{
    public interface IWorkFlowService
    {
        Task<IEnumerable<WorkFlow>> GetInstalledWorkFlowCollectionAsync(Guid companyID);

        Task<IEnumerable<WorkFlow>> GetDesigningWorkFlowCollectionAsync(Guid companyID);

        Task<Models.WorkFlow> GetInstalledWorkFlowByKeyAsync(Guid flowId);
    }
}
