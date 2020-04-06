using System;
using System.Linq;
using ZDY.DMS.Services.WorkFlowService.Enums;
using ZDY.DMS.Services.WorkFlowService.Models;

namespace ZDY.DMS.Services.WorkFlowService.Core.Extensions
{
    public static class WorkFlowExtensions
    {
        public static bool Is(this WorkFlow flow, params WorkFlowState[] states)
        {
            return states.Contains((WorkFlowState)flow.Type);
        }

        public static bool IsNot(this WorkFlow flow, params WorkFlowState[] states)
        {
            return !flow.Is(states);
        }

        public static bool IsInstalled(this WorkFlow flow)
        {
            return flow.Is(WorkFlowState.Installed);
        }

        public static bool IsNotInstalled(this WorkFlow flow)
        {
            return !flow.IsInstalled();
        }
    }
}
