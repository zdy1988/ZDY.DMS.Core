using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.Metronic.UI.TagHelpers
{
    [HtmlTargetElement("a", Attributes = "route-to")]
    [HtmlTargetElement("button", Attributes = "route-to")]
    public class RouteToTagHelper : TagHelper
    {
        public string RouteTo { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!String.IsNullOrWhiteSpace(RouteTo))
            {
                output.Attributes.RemoveAll("data-url");

                output.Attributes.RemoveAll("data-bind");

                if (RouteTo.Contains("="))
                {
                    output.Attributes.Add("data-bind", "click:$root.goto, attr: {'data-url': " + RouteTo + "}");
                }
                else
                {
                    output.Attributes.Add("data-url", RouteTo);

                    output.Attributes.Add("data-bind", "click:$root.goto");
                }
            }
        }
    }
}
