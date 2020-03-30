using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.Services.WorkFlowService.DataObjects
{
    public class WorkFlowDefinition
    {       
        public string Name { get; set; }
        public List<WorkFlowStep> Steps { get; set; }
        public List<WrokFlowTransit> Transits { get; set; }
    }
}
