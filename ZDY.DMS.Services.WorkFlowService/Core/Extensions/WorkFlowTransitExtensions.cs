using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using ZDY.DMS.Services.Common.ServiceContracts;
using ZDY.DMS.Services.WorkFlowService.Enums;
using ZDY.DMS.Services.WorkFlowService.Core.Models;

namespace ZDY.DMS.Services.WorkFlowService.Core.Extensions
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

        public static bool IsNoCondition(this WrokFlowTransit transit)
        {
            return transit.ConditionIs(WorkFlowTransitConditionKinds.None);
        }

        public static bool IsConditionPassed(this WrokFlowTransit transit, JObject data)
        {
            if (String.IsNullOrEmpty(transit.ConditionValue))
            {
                return true;
            }

            switch ((WorkFlowTransitConditionKinds)transit.ConditionType)
            {
                case WorkFlowTransitConditionKinds.Data:

                    return transit.IsDataConditionPassed(data);

                case WorkFlowTransitConditionKinds.Method:

                    return transit.IsMethodConditionPassed(data);

                case WorkFlowTransitConditionKinds.SQL:

                    return transit.IsSQLConditionPassed(data);

                default:

                    return false;
            }
        }

        public static string GetConditionTypeName(this WrokFlowTransit transit)
        {
            return ServiceLocator.GetService<IDictionaryService>().GetDictionaryName("WorkFlowTransitConditionKinds", transit.ConditionType.ToString());
        }
    }
}
