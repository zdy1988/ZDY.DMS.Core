using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.Services.WorkFlowService.Events;

namespace ZDY.DMS.Services.WorkFlowService.Test
{
    public class Event
    {
        public ExecuteSubmitBeforeEventResults SubmitBefore(WorkFlowCustomEventArgs data)
        {
            return new ExecuteSubmitBeforeEventResults();
        }

        public ExecuteSubmitAfterEventResults SubmitAfter(WorkFlowCustomEventArgs data)
        {
            return new ExecuteSubmitAfterEventResults();
        }

        public ExecuteBackBeforeEventResults BackBefore(WorkFlowCustomEventArgs data)
        {
            return new ExecuteBackBeforeEventResults();
        }

        public ExecuteBackAfterEventResults BackAfter(WorkFlowCustomEventArgs data)
        {
            return new ExecuteBackAfterEventResults();
        }

        public WorkFlowCustomEventResults SubFlowActivationBefore(WorkFlowCustomEventArgs data)
        {
            return new WorkFlowCustomEventResults();
        }

        public WorkFlowCustomEventResults SubFlowActivationAfter(WorkFlowCustomEventArgs data)
        {
            return new WorkFlowCustomEventResults();
        }

        public WorkFlowCustomEventResults SubFlowFinished(WorkFlowCustomEventArgs data)
        {
            return new WorkFlowCustomEventResults();
        }
    }
}
