using System;
using System.Collections.Generic;
using System.Linq;
using ZDY.DMS.Services.WorkFlowService.DataObjects;

namespace ZDY.DMS.Services.WorkFlowService
{
    public static class WorkFlowDefinitionExtensions
    {
        public static WorkFlowStep GetStep(this WorkFlowDefinition definition, Guid stepId)
        {
            return definition.Steps.Find(t => t.StepId == stepId);
        }

        public static List<WorkFlowStep> GetPrevSteps(this WorkFlowDefinition definition, Guid stepId)
        {
            List<WorkFlowStep> stepList = new List<WorkFlowStep>();
            var transits = definition.Transits.FindAll(p => p.ToStepId == stepId);
            if (transits.Count() > 0)
            {
                var fromStepIdArray = transits.Select(t => t.FromStepId).ToArray();
                stepList = definition.Steps.Where(p => fromStepIdArray.Contains(p.StepId)).ToList();
            }
            return stepList;
        }

        public static List<WorkFlowStep> GetNextSteps(this WorkFlowDefinition definition, Guid stepId)
        {
            List<WorkFlowStep> stepList = new List<WorkFlowStep>();
            var transits = definition.Transits.Where(p => p.FromStepId == stepId);
            if (transits.Count() > 0)
            {
                var toStepIdArray = transits.Select(t => t.ToStepId).ToArray();
                stepList = definition.Steps.Where(p => toStepIdArray.Contains(p.StepId)).ToList();
            }
            return stepList;
        }

        public static WorkFlowStep GetFirstStep(this WorkFlowDefinition definition)
        {
            return definition.Steps.Find(t => t.StepId == WorkFlowConstant.StartStepId);
        }

        public static WorkFlowStep GetLastStep(this WorkFlowDefinition definition)
        {
            return definition.Steps.Find(t => t.StepId == WorkFlowConstant.EndStepId);
        }

        public static WrokFlowTransit GetTransit(this WorkFlowDefinition definition, Guid transitId)
        {
            return definition.Transits.Find(t => t.TransitId == transitId);
        }

        public static WrokFlowTransit GetTransit(this WorkFlowDefinition definition, Guid formStepId, Guid toStepId)
        {
            return definition.Transits.Find(t => t.FromStepId == formStepId && t.ToStepId == toStepId);
        }
    }
}
