using System;
using System.Collections.Generic;
using System.Linq;

namespace ZDY.DMS.Metronic.TagHelpers.Untils
{
    public class DataRuleBuilder
    {
        public static string Build(params DataRuleName[] ruleNames)
        {
            var names = ruleNames.Where(t => t.IsAppend == true).Select(t => t.RuleName);
            return names.Count() > 0 ? String.Join(",", names) : "";
        }
    }
}
