using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.Services.WorkFlowService.DataObjects
{
    public class WrokFlowTransit
    {
        public Guid TransitId { get; set; }
        public string TransitName { get; set; }
        public Guid FromStepId { get; set; }
        public Guid ToStepId { get; set; }
        public int ConditionType { get; set; }
        public string ConditionValue { get; set; }

    }
}
