using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.Services.WorkFlowService.Models;

namespace ZDY.DMS.Services.WorkFlowService.Core.Comparers
{
    public class WorkFlowReceiverTaskComparer : IEqualityComparer<WorkFlowTask>
    {
        public bool Equals(WorkFlowTask x, WorkFlowTask y)
        {
            if (x == null)
                return y == null;

            return x.ReceiverId == y.ReceiverId && x.StepId == y.StepId && x.FlowId == y.FlowId && x.InstanceId == y.InstanceId;
        }


        public int GetHashCode(WorkFlowTask obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
