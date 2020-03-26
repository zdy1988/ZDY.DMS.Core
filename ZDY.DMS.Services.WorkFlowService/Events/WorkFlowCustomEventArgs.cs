using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.Services.WorkFlowService.Events
{
    public class WorkFlowCustomEventArgs : EventArgs
    {
        public Guid FlowId { get; set; }

        public Guid TaskId { get; set; }

        public Guid StepId { get; set; }

        public Guid GroupId { get; set; }

        public Guid InstanceId { get; set; }
    }
}
