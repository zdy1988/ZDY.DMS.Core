using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZDY.DMS.Services.WorkFlowService.Enums;
using ZDY.DMS.Services.WorkFlowService.Models;

namespace ZDY.DMS.Services.WorkFlowService.Core.Interfaces
{
    public interface IPersistenceProvider
    {
        Task<WorkFlow> GetWorkFlowAsync(Guid flowId);

        Task<WorkFlowForm> GetWorkFlowFormAsync(Guid formId);

        Task<WorkFlowInstance> GetWorkFlowInstanceAsync(Guid instanceId);

        Task CreateInstanceAsync(WorkFlowInstance instance);

        Task RemoveInstanceAsync(Guid instanceId);

        Task UpdateInstanceAsync(WorkFlowInstance instance);

        Task<WorkFlowTask> GetWorkFlowTaskAsync(Guid taskId);

        Task<WorkFlowTask> GetRootInstanceTaskAsync(Guid instanceId, Guid groupId);

        Task CreateTaskAsync(WorkFlowTask task);

        Task RemoveTaskAsync(WorkFlowTask task);

        Task UpdateTaskAsync(WorkFlowTask task);

        Task<List<WorkFlowTask>> GetAllStepDistributionTaskAsync(Guid stepId, Guid flowId, Guid instanceId, Guid groupId, int sort);

        Task<List<WorkFlowTask>> GetAllStepDistributionNewestTaskAsync(Guid stepId, Guid flowId, Guid instanceId, Guid groupId);

        Task<List<WorkFlowTask>> GetAllStepDistributionNotExecuteTaskAsync(Guid[] stepArray, Guid flowId, Guid instanceId, Guid groupId);

        Task<bool> IsTheReceiverHasNotExecuteTask(Guid flowId, Guid instanceId, Guid stepId, Guid groupId, Guid userId);

        Task<List<WorkFlowTask>> GetAllTemporaryTaskAsync(Guid instanceId);

        Task<List<WorkFlowTask>> GetAllTemporaryTaskAsync(Guid instanceId, params Guid[] stepArray);

        Task<List<WorkFlowTask>> GetAllTaskAsync(Guid instanceId);
    }
}
