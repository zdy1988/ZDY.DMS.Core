using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.Services.WorkFlowService.Models;

namespace ZDY.DMS.Services.WorkFlowService.Core.Interfaces
{
    public interface ITaskProvider
    {
        /// <summary>
        /// 获取某些步骤在某阶段分发的任务
        /// </summary>
        Task<List<WorkFlowTask>> GetDistributionTaskAsync(Guid instanceId, int sort, params Guid[] stepArray);

        /// <summary>
        /// 获取某些步骤最新分发的任务
        /// </summary>
        Task<List<WorkFlowTask>> GetNewestDistributionTaskAsync(Guid instanceId, params Guid[] stepArray);

        /// <summary>
        /// 获取某些步骤没有处理过的任务
        /// </summary>
        Task<List<WorkFlowTask>> GetNotExecuteDistributionTaskAsync(Guid instanceId, params Guid[] stepArray);

        /// <summary>
        /// 获取某个实例下所有的临时任务
        /// </summary>
        Task<List<WorkFlowTask>> GetTemporaryTaskAsync(Guid instanceId);

        /// <summary>
        /// 获取某个实例下规定步骤范围内的临时任务
        /// </summary>
        Task<List<WorkFlowTask>> GetTemporaryTaskAsync(Guid instanceId, params Guid[] stepArray);

        /// <summary>
        /// 判断某个用户是否在某个步骤还有未处理的任务
        /// </summary>
        Task<bool> IsTheReceiverHasNotExecuteTask(Guid instanceId, Guid receiverId, params Guid[] stepArray);
    }
}
