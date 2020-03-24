using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.Metronic.TagHelpers.Tabs
{
    public class TabsContext
    {
        public List<Tuple<string, IHtmlContent>> TabContents { get; set; } = new List<Tuple<string, IHtmlContent>>();
    }
}
