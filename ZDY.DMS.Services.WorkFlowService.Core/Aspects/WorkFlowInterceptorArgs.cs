using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.Services.WorkFlowService.Models;

namespace ZDY.DMS.Services.WorkFlowService.Core.Aspects
{
    public class WorkFlowInterceptorArgs
    {
        public WorkFlowTask Task { get; set; }

        public WorkFlowInterceptorArgs(WorkFlowTask task)
        {
            this.Task = task;
        }
    }
}
