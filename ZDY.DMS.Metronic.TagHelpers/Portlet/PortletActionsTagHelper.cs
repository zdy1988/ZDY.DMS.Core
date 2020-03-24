using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZDY.DMS.Metronic.TagHelpers.Portlet
{
    [HtmlTargetElement("portlet-actions", ParentTag = "portlet")]
    public class PortletActionsTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();

            var content = new TagBuilder("div");
            content.Attributes.Add("class", "actions");

            content.InnerHtml.AppendHtml(childContent);

            var portletContext = (PortletContext)context.Items[typeof(PortletTagHelper)];
            portletContext.Actions = content;

            output.SuppressOutput();
        }
    }
}
