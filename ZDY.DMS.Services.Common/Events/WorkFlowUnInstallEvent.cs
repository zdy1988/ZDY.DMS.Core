using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.Events;

namespace ZDY.DMS.Services.Common.Events
{
    public class WorkFlowUnInstallEvent : Event
    {
        /// <summary>
        /// 需要卸载的流程Id
        /// </summary>
        public Guid FlowId { get; set; }

        public WorkFlowUnInstallEvent(Guid flowId)
        {
            this.FlowId = flowId;
        }
    }
}
