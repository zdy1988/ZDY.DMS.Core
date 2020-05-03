using System;
using System.Linq;
using ZDY.DMS.Services.WorkFlowService.Core.Models;
using ZDY.DMS.Services.WorkFlowService.Core.Enums;

namespace ZDY.DMS.Services.WorkFlowService.Core.Extensions
{
    public static class WorkFlowExecuteExtensions
    {
        public static bool IsExecuteBy(this WorkFlowExecute execute, params WorkFlowExecuteKinds[] types)
        {
            return types.Contains(execute.ExecuteType);
        }

        public static bool IsSubmit(this WorkFlowExecute execute)
        {
            return execute.IsExecuteBy(WorkFlowExecuteKinds.Submit);
        }

        public static bool IsBack(this WorkFlowExecute execute)
        {
            return execute.IsExecuteBy(WorkFlowExecuteKinds.Back);
        }

        public static bool IsRedirect(this WorkFlowExecute execute)
        {
            return execute.IsExecuteBy(WorkFlowExecuteKinds.Redirect);
        }
    }
}
