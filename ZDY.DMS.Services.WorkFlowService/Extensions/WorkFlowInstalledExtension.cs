using System;
using System.Collections.Generic;
using System.Linq;
using ZDY.DMS.Services.WorkFlowService.DataObjects;

namespace ZDY.DMS.Services.WorkFlowService
{
    public static class WorkFlowInstalledExtension
    {
        public static WorkFlowStep GetStep(this WorkFlowInstalled workFlowInstalled, Guid stepId)
        {
            return workFlowInstalled.Steps.Find(t => t.StepId == stepId);
        }

        public static List<WorkFlowStep> GetPrevSteps(this WorkFlowInstalled workFlowInstalled, Guid stepId)
        {
            List<WorkFlowStep> stepList = new List<WorkFlowStep>();
            var transits = workFlowInstalled.Transits.FindAll(p => p.ToStepId == stepId);
            if (transits.Count() > 0)
            {
                var fromStepIDArray = transits.Select(t => t.FromStepId).ToArray();
                stepList = workFlowInstalled.Steps.Where(p => fromStepIDArray.Contains(p.StepId)).ToList();
            }
            return stepList;
        }

        public static List<WorkFlowStep> GetNextSteps(this WorkFlowInstalled workFlowInstalled, Guid stepId)
        {
            List<WorkFlowStep> stepList = new List<WorkFlowStep>();
            var transits = workFlowInstalled.Transits.Where(p => p.FromStepId == stepId);
            if (transits.Count() > 0)
            {
                var toStepIDArray = transits.Select(t => t.ToStepId).ToArray();
                stepList = workFlowInstalled.Steps.Where(p => toStepIDArray.Contains(p.StepId)).ToList();
            }
            return stepList;
        }

        public static WorkFlowStep GetFirstStep(this WorkFlowInstalled workFlowInstalled)
        {
            return workFlowInstalled.Steps.Find(t => t.StepId == WorkFlowAnalysis.StartStepID);
        }

        public static WorkFlowStep GetLastStep(this WorkFlowInstalled workFlowInstalled)
        {
            return workFlowInstalled.Steps.Find(t => t.StepId == WorkFlowAnalysis.EndStepID);
        }

        public static WrokFlowTransit GetTransit(this WorkFlowInstalled workFlowInstalled, Guid transitId)
        {
            return workFlowInstalled.Transits.Find(t => t.TransitId == transitId);
        }

        public static WrokFlowTransit GetTransit(this WorkFlowInstalled workFlowInstalled, Guid formStepId, Guid toStepId)
        {
            return workFlowInstalled.Transits.Find(t => t.FromStepId == formStepId && t.ToStepId == toStepId);
        }
    }
}
