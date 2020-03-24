using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZDY.DMS.Metronic.TagHelpers.FormBuilder
{
    [HtmlTargetElement("form-actions", ParentTag = "form-builder")]
    public class FromActionsTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();

            output.TagName = "div";

            output.Attributes.Add("class", "form-actions");

            var content = $@"<div class='row'>
                                <div class='col-md-12 text-right'>
                                     {childContent.GetContent()}
                                </div>
                            </div>";

            output.Content.SetHtmlContent(content);
        }
    }
}
