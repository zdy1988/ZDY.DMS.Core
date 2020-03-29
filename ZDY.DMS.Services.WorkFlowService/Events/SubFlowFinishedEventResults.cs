using ZDY.DMS.Services.WorkFlowService.Models;

namespace ZDY.DMS.Services.WorkFlowService.Events
{
    public class SubFlowFinishedEventResults : WorkFlowCustomEventResults
    {
        /// <summary>
        /// 子流程实例
        /// </summary>
        public WorkFlowInstance SubFlowInstance { get; set; }
    }
}
