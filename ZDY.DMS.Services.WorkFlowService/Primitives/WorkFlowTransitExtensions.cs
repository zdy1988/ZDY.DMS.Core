using System;
using System.Linq;
using ZDY.DMS.Services.Common.ServiceContracts;
using ZDY.DMS.Services.WorkFlowService.DataObjects;
using ZDY.DMS.Services.WorkFlowService.Enums;

namespace ZDY.DMS.Services.WorkFlowService
{
    public static class WorkFlowTransitExtensions
    {
        public static bool ConditionIs(this WrokFlowTransit transit, params WorkFlowTransitConditionKinds[] types)
        {
            return types.Contains((WorkFlowTransitConditionKinds)transit.ConditionType);
        }

        public static bool ConditionIsNot(this WrokFlowTransit transit, params WorkFlowTransitConditionKinds[] types)
        {
            return !transit.ConditionIs(types);
        }

        public static string GetConditionTypeName(this WrokFlowTransit transit)
        {
            return ServiceLocator.GetService<IDictionaryService>().GetDictionaryName("WorkFlowTransitConditionKinds", transit.ConditionType.ToString());
        }
    }
}
