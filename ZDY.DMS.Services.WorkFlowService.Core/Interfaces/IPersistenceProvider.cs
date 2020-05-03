using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using ZDY.DMS.Services.WorkFlowService.Core.Enums;
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

        Task<List<WorkFlowTask>> GetAllTaskAsync(Guid instanceId);
    }
}
