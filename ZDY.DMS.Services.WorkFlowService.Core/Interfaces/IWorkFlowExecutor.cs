using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.Services.WorkFlowService.Core.Models;
using ZDY.DMS.Services.WorkFlowService.Models;

namespace ZDY.DMS.Services.WorkFlowService.Core.Interfaces
{
    public interface IWorkFlowExecutor
    {
        Task ExecuteAsync(WorkFlowExecute execute);

        Task ExecuteStartAsync(WorkFlowInstance instance, Guid groupId);

        Task<Dictionary<int, List<WorkFlowStep>>> GetWorkFlowProcessAsync(WorkFlowInstance instance);
    }
}
