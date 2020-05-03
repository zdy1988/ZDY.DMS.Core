using System;
using System.Data;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using ZDY.DMS.Services.WorkFlowService.Core.Models;

namespace ZDY.DMS.Services.WorkFlowService.Core.Extensions
{
    public static class WorkFlowTransitConditionPassed
    {
        public static bool IsDataConditionPassed(this WrokFlowTransit transit, JObject data)
        {
            bool isPassed;

            try
            {
                var condition = transit.ConditionValue.Trim();

                var reg = new Regex(@"{(\w+?)}");

                var matches = reg.Matches(condition);

                var query = condition;

                foreach (Match matche in matches)
                {
                    string field = matche.Value.Trim('{', '}').ToLower();

                    if (data[field] != null)
                    {
                        query = query.Replace(matche.Value, data[field].ToString());
                    }
                }

                isPassed = (bool)(new DataTable()).Compute(query, "");
            }
            catch
            {
                isPassed = false;
            }

            return isPassed;
        }

        public static bool IsMethodConditionPassed(this WrokFlowTransit transit, JObject data)
        {
            bool isPassed;

            try
            {
                var condition = transit.ConditionValue.Trim();

                WorkFlowConstant.ExecuteMethod<object, bool>(condition, data, out isPassed);
            }
            catch (Exception e)
            {
                isPassed = false;

                throw e;
            }

            return isPassed;
        }

        public static bool IsSQLConditionPassed(this WrokFlowTransit transit, JObject data)
        {
            //bool isPassed;

            //try
            //{
            //    var condition = transit.ConditionValue.Trim();
            //    var reg = new Regex(@"{(\w+?)}");
            //    var matches = reg.Matches(condition);
            //    var query = condition;
            //    foreach (Match matche in matches)
            //    {
            //        string field = matche.Value.Trim('{', '}').ToLower();

            //        if (data[field] != null)
            //        {
            //            query = query.Replace(matche.Value, data[field].ToString());
            //        }
            //    }

            //    isPassed = (Int64)dataTableGateway.ExecuteScalar(query) == 1;
            //}
            //catch
            //{
            //    isPassed = false;
            //}

            //return isPassed;
            return true;
        }
    }
}
