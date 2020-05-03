using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.Services.WorkFlowService.Models;

namespace ZDY.DMS.Services.WorkFlowService.ServiceContracts
{
    public interface IWorkFlowTaskService
    {
        Task<WorkFlowTask> GetWorkFlowTaskByKeyAsync(Guid taskId);

        /// <summary>
        /// 当任务被打开时，记录状态和打开时间
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        Task WorkFlowTaskOpenedAsync(Guid taskId);
    }
}
