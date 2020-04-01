using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.Services.WorkFlowService.Models;

namespace ZDY.DMS.Services.WorkFlowService.ServiceContracts
{
    public interface IWorkFlowInstanceService
    {
        Task<WorkFlowInstance> GetWorkFlowInstanceByKeyAsync(Guid instanceId);
    }
}
