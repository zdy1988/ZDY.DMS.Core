using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.Events;

namespace ZDY.DMS.Services.Shared.Events
{
    public class WorkFlowUnInstalledEvent : Event
    {
        /// <summary>
        /// 需要卸载的流程Id
        /// </summary>
        public Guid FlowId { get; set; }

        public WorkFlowUnInstalledEvent(Guid flowId)
        {
            this.FlowId = flowId;
        }
    }
}
