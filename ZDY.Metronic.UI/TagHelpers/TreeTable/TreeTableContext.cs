using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;

namespace ZDY.Metronic.UI.TagHelpers
{
    internal class TreeTableContext : IHelperContext
    {
        internal List<Tuple<TreeTableTemplateColumnTagHelper, IHtmlContent>> TreeTableTemplateColumns { get; set; } = new List<Tuple<TreeTableTemplateColumnTagHelper, IHtmlContent>>();
    }
}
