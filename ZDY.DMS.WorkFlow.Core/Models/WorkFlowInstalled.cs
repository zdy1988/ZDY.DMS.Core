using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.WorkFlow.Core.Models
{
    public class WorkflowInstalled
    {       
        public string Name { get; set; }
        public List<WorkflowStep> Steps { get; set; }
        public List<WorkflowTransit> Transits { get; set; }
    }
}
