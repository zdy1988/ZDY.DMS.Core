using System;
using ZDY.DMS.Services.WorkFlowService.Core.Aspects;

namespace ZDY.DMS.Services.WorkFlowService.CutomSimple
{
    public class Interceptor
    {
        public WorkFlowInterceptorResults SubmitBefore(WorkFlowInterceptorArgs args)
        {
            return new WorkFlowInterceptorResults();
        }

        public WorkFlowInterceptorResults SubmitAfter(WorkFlowInterceptorArgs args)
        {
            return new WorkFlowInterceptorResults();
        }

        public WorkFlowInterceptorResults BackBefore(WorkFlowInterceptorArgs args)
        {
            return new WorkFlowInterceptorResults();
        }

        public WorkFlowInterceptorResults BackAfter(WorkFlowInterceptorArgs args)
        {
            return new WorkFlowInterceptorResults();
        }

        public WorkFlowInterceptorResults SubFlowActivationBefore(WorkFlowInterceptorArgs args)
        {
            return new WorkFlowInterceptorResults();
        }

        public WorkFlowInterceptorResults SubFlowActivationAfter(WorkFlowInterceptorArgs args)
        {
            return new WorkFlowInterceptorResults();
        }

        public WorkFlowInterceptorResults SubFlowFinished(WorkFlowInterceptorArgs args)
        {
            return new WorkFlowInterceptorResults();
        }
    }
}
