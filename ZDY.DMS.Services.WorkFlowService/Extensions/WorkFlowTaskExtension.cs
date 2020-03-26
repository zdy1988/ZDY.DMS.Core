using System;
using System.Linq;
using ZDY.DMS.Domain.Enums;
using ZDY.DMS.Extensions.DependencyInjection.Autofac;
using ZDY.DMS.Models;
using ZDY.DMS.ServiceContracts;

namespace ZDY.DMS.Services.WorkFlowService
{
    public static class WorkFlowTaskExtension
    {
        public static bool IsExecute(this WorkFlowTask task)
        {
            return ((WorkFlowTaskState)task.State).IsExecute();
        }

        public static bool IsNotExecute(this WorkFlowTask task)
        {
            return !task.IsExecute();
        }

        public static bool IsFirstStep(this WorkFlowTask task)
        {
            return task.StepId == WorkFlowAnalysis.StartStepId;
        }

        public static bool IsLastStep(this WorkFlowTask task)
        {
            return task.StepId == WorkFlowAnalysis.EndStepId;
        }

        public static bool IsStart(this WorkFlowTask task)
        {
            return task.StepId == WorkFlowAnalysis.StartStepId;
        }

        public static bool IsEnd(this WorkFlowTask task)
        {
            return task.StepId == WorkFlowAnalysis.EndStepId;
        }

        public static bool Is(this WorkFlowTask task, params WorkFlowTaskKinds[] types)
        {
            return types.Contains((WorkFlowTaskKinds)task.Type);
        }

        public static bool IsNot(this WorkFlowTask task, params WorkFlowTaskKinds[] types)
        {
            return !types.Contains((WorkFlowTaskKinds)task.Type);
        }

        public static bool Is(this WorkFlowTask task, params WorkFlowTaskState[] state)
        {
            return state.Contains((WorkFlowTaskState)task.State);
        }

        public static bool IsNot(this WorkFlowTask task, params WorkFlowTaskState[] state)
        {
            return !state.Contains((WorkFlowTaskState)task.State);
        }

        public static bool IsExecute(this WorkFlowTaskState state)
        {
            return state == WorkFlowTaskState.Handled || state == WorkFlowTaskState.Returned || state == WorkFlowTaskState.HandledByOthers || state == WorkFlowTaskState.ReturnedByOthers;
        }

        public static bool IsNotExecute(this WorkFlowTaskState state)
        {
            return !state.IsExecute();
        }

        public static string GetTaskStateName(this WorkFlowTask task)
        {
            
            return ServiceLocator.GetService<IDictionaryService>().GetDictionaryName("WorkFlowTaskState", task.State.ToString());
        }
    }
}
