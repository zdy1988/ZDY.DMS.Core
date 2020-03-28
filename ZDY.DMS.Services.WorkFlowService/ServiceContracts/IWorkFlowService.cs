using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.Models;

namespace ZDY.DMS.Services.WorkFlowService.ServiceContracts
{
    public interface IWorkFlowService
    {
        Task<IEnumerable<Models.WorkFlow>> GetInstalledWorkFlows(Guid companyID);

        Task<IEnumerable<Models.WorkFlow>> GetDesigningWorkFlows(Guid companyID);
    }
}
