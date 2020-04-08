using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.Services.WorkFlowService.Core.Extensions;
using ZDY.DMS.Services.WorkFlowService.Core.Interfaces;
using ZDY.DMS.Services.WorkFlowService.Enums;
using ZDY.DMS.Services.WorkFlowService.Models;

namespace ZDY.DMS.Services.WorkFlowService.Core.Services
{
    public class TaskProvider : ITaskProvider
    {
        private readonly IPersistenceProvider persistenceProvider;

        public TaskProvider(IPersistenceProvider persistenceProvider)
        {
            this.persistenceProvider = persistenceProvider;
        }

        /// <summary>
        /// 获取某些步骤在某阶段分发的任务
        /// </summary>
        public async Task<List<WorkFlowTask>> GetAllStepDistributionTaskAsync(Guid instanceId, int sort, params Guid[] stepArray)
        {
            var list = await this.persistenceProvider.GetAllTaskAsync(instanceId);

            return list.FindAll(t => t.Sort == sort && stepArray.Contains(t.StepId));
        }

        /// <summary>
        /// 获取某些步骤最新分发的任务
        /// </summary>
        public async Task<List<WorkFlowTask>> GetAllStepDistributionNewestTaskAsync(Guid instanceId, params Guid[] stepArray)
        {
            var list = await this.persistenceProvider.GetAllTaskAsync(instanceId);

            var maxSort = list.Max(t => t.Sort);

            return list.FindAll(t => t.Sort == maxSort && stepArray.Contains(t.StepId));
        }

        /// <summary>
        /// 获取某些步骤没有处理过的任务
        /// </summary>
        public async Task<List<WorkFlowTask>> GetAllStepDistributionNotExecuteTaskAsync(Guid instanceId, params Guid[] stepArray)
        {
            var list = await this.persistenceProvider.GetAllTaskAsync(instanceId);

            return list.FindAll(t => t.IsNotExecute() && stepArray.Contains(t.StepId));
        }

        /// <summary>
        /// 获取某个实例下所有的临时任务
        /// </summary>
        public async Task<List<WorkFlowTask>> GetTemporaryTaskAsync(Guid instanceId)
        {
            var list = await this.persistenceProvider.GetAllTaskAsync(instanceId);

            return list.FindAll(t => t.Is(WorkFlowTaskState.Waiting));
        }

        /// <summary>
        /// 获取某个实例下规定步骤范围内的临时任务
        /// </summary>
        public async Task<List<WorkFlowTask>> GetTemporaryTaskAsync(Guid instanceId, params Guid[] stepArray)
        {
            var list = await this.persistenceProvider.GetAllTaskAsync(instanceId);

            return list.FindAll(t => t.Is(WorkFlowTaskState.Waiting) && stepArray.Contains(t.StepId));
        }

        /// <summary>
        /// 判断某个用户是否在某个步骤还有未处理的任务
        /// </summary>
        public async Task<bool> IsTheReceiverHasNotExecuteTask(Guid instanceId, Guid receiverId, params Guid[] stepArray)
        {
            var list = await this.persistenceProvider.GetAllTaskAsync(instanceId);

            return list.Where(t => t.IsNotExecute() && t.ReceiverId == receiverId && stepArray.Contains(t.StepId)).Any();
        }
    }
}
