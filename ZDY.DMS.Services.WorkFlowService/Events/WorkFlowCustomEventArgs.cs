using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.Services.WorkFlowService.Events
{
    public class WorkFlowCustomEventArgs : EventArgs
    {
        public Guid FlowID { get; set; }

        public Guid TaskID { get; set; }

        public Guid StepID { get; set; }

        public Guid GroupID { get; set; }

        public Guid InstanceID { get; set; }
    }
}
