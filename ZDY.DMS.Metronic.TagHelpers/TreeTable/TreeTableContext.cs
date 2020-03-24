using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.Metronic.TagHelpers.TreeTable
{
    public class TreeTableContext
    {
        public List<Tuple<TreeTableActionTagHelper, IHtmlContent>> TreeTableActions { get; set; } = new List<Tuple<TreeTableActionTagHelper, IHtmlContent>>();
    }
}
