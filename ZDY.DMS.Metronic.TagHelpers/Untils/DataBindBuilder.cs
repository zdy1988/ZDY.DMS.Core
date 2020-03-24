using System;
using System.Collections.Generic;
using System.Linq;

namespace ZDY.DMS.Metronic.TagHelpers.Untils
{
    public class DataBindBuilder
    {
        public static string Build(params DataBindName[] bindNames)
        {
            var names = bindNames.Where(t => t.IsAppend == true).Select(t => t.BindName);
            return names.Count() > 0 ? String.Join(",", names) : "";
        }
    }
}
