using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZDY.DMS.Services.WorkFlowService.Core.Models;
using ZDY.DMS.Services.WorkFlowService.Models;

namespace ZDY.DMS.Services.WorkFlowService.ServiceContracts
{
    public interface IWorkFlowHostService
    {
        Task StartUp(WorkFlowInstance instance);

        Task Execute(WorkFlowExecute execute);

        Task<List<WorkFlowTask>> GetWorkFlowProcessTaskListAsync(Guid instanceId);

        Task<List<WorkFlowTask>> GetWorkFlowCommentTaskListAsync(Guid instanceId);

        Task<Dictionary<int, List<WorkFlowStep>>> GetWorkFlowProcessAsync(WorkFlowInstance instance);
    }
}
