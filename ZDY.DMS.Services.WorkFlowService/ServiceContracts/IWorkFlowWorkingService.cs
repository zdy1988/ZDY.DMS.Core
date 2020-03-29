using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZDY.DMS.Services.WorkFlowService.DataObjects;
using ZDY.DMS.Services.WorkFlowService.Models;

namespace ZDY.DMS.Services.WorkFlowService.ServiceContracts
{
    public interface IWorkFlowWorkingService
    {
        Task StartUp(WorkFlowInstance instance);

        Task Execute(WorkFlowExecute execute);

        Task<List<WorkFlowTask>> GetWorkFlowProcessAsync(WorkFlowInstance instance);

        Task<List<WorkFlowTask>> GetWorkFlowCommentsAsync(WorkFlowInstance instance);

        Task<Dictionary<int, List<WorkFlowStep>>> GetWorkFlowProcessStatesAsync(WorkFlowInstance instance);
    }
}
