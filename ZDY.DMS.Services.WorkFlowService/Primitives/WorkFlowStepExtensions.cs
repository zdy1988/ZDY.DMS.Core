using System;
using System.Linq;
using ZDY.DMS.Services.Common.ServiceContracts;
using ZDY.DMS.Services.WorkFlowService.DataObjects;
using ZDY.DMS.Services.WorkFlowService.Enums;

namespace ZDY.DMS.Services.WorkFlowService
{
    public static class WorkFlowStepExtensions
    {
        public static bool IsFirstStep(this WorkFlowStep step)
        {
            return step.StepId == WorkFlowConstant.StartStepId;
        }

        public static bool IsLastStep(this WorkFlowStep step)
        {
            return step.StepId == WorkFlowConstant.EndStepId;
        }

        public static bool IsStart(this WorkFlowStep step)
        {
            return step.StepId == WorkFlowConstant.StartStepId;
        }

        public static bool IsEnd(this WorkFlowStep step)
        {
            return step.StepId == WorkFlowConstant.EndStepId;
        }

        public static bool Is(this WorkFlowStep step, params WorkFlowStepKinds[] types)
        {
            return types.Contains((WorkFlowStepKinds)step.StepType);
        }

        public static bool IsSubFlowStep(this WorkFlowStep step)
        {
            return step.Is(WorkFlowStepKinds.SubFlow);
        }

        public static bool IsControlBy(this WorkFlowStep step, params WorkFlowControlKinds[] types)
        {
            return types.Contains((WorkFlowControlKinds)step.FlowControl);
        }

        public static bool IsHandleBy(this WorkFlowStep step, params WorkFlowHandlerKinds[] types)
        {
            return types.Contains((WorkFlowHandlerKinds)step.HandlerType);
        }

        public static bool IsHandleTacticBy(this WorkFlowStep step, params WorkFlowHandleTacticKinds[] tactics)
        {
            return tactics.Contains((WorkFlowHandleTacticKinds)step.HandleTactic);
        }

        public static bool IsBackBy(this WorkFlowStep step, params WorkFlowBackKinds[] types)
        {
            return types.Contains((WorkFlowBackKinds)step.BackType);
        }

        public static bool IsBackTacticBy(this WorkFlowStep step, params WorkFlowBackTacticKinds[] tactics)
        {
            return tactics.Contains((WorkFlowBackTacticKinds)step.BackTactic);
        }

        public static bool IsCountersignatureTacticBy(this WorkFlowStep step, params WorkFlowCountersignatureTacticKinds[] tactics)
        {
            return tactics.Contains((WorkFlowCountersignatureTacticKinds)step.CountersignatureTactic);
        }

        public static bool IsNeedCountersignature(this WorkFlowStep step)
        {
            return !step.IsCountersignatureTacticBy(WorkFlowCountersignatureTacticKinds.NoCountersignature);
        }

        public static bool IsNotNeedCountersignature(this WorkFlowStep step)
        {
            return !step.IsNeedCountersignature();
        }

        public static bool IsSignatureBy(this WorkFlowStep step, params WorkFlowSignatureKinds[] types)
        {
            return types.Contains((WorkFlowSignatureKinds)step.SignatureType);
        }

        public static bool IsNeedSignature(this WorkFlowStep step)
        {
            return step.IsSignatureBy(WorkFlowSignatureKinds.CommentAndSignature);
        }

        public static bool IsSubFlowTacticBy(this WorkFlowStep step, params WorkFlowSubFlowTacticKinds[] types)
        {
            return types.Contains((WorkFlowSubFlowTacticKinds)step.SubFlowTactic);
        }

        public static bool IsExecuteAfterSubFlowEnd(this WorkFlowStep step)
        {
            return !step.IsSubFlowTacticBy(WorkFlowSubFlowTacticKinds.SubFlowStarted);
        }

        public static string GetHandleTypeName(this WorkFlowStep step)
        {
            return ServiceLocator.GetService<IDictionaryService>().GetDictionaryName("WorkFlowHandlerKinds", step.HandlerType.ToString());
        }
    }
}
