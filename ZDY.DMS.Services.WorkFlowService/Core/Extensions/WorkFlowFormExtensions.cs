using System;
using System.Linq;
using ZDY.DMS.Services.WorkFlowService.Enums;
using ZDY.DMS.Services.WorkFlowService.Models;

namespace ZDY.DMS.Services.WorkFlowService.Core.Extensions
{
    public static class WorkFlowFormExtensions
    {
        public static bool Is(this WorkFlowForm form, params WorkFlowFormState[] states)
        {
            return states.Contains((WorkFlowFormState)form.Type);
        }

        public static bool IsNot(this WorkFlowForm form, params WorkFlowFormState[] states)
        {
            return !form.Is(states);
        }

        public static bool IsPublished(this WorkFlowForm form)
        {
            return form.Is(WorkFlowFormState.Published);
        }

        public static bool IsNotPublished(this WorkFlowForm form)
        {
            return !form.IsPublished();
        }
    }
}
