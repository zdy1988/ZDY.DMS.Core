using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.Metronic.TagHelpers.Untils;

namespace ZDY.DMS.Metronic.TagHelpers.FormBuilder
{
    [HtmlTargetElement("form-builder")]
    public class FormBuilderTagHelper : TagHelper
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();
        public string Action { get; set; } = "#";
        public string Method { get; set; } = "post";
        public string ClassNames { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();

            output.TagName = "form";

            var id = $"form_{ID}";

            output.Attributes.Add("class", ClassNames);

            output.Attributes.Add("id", id);

            output.Attributes.Add("action", Action);

            output.Attributes.Add("method", Method);

            var content = $@"<div class='form-body'>
                                 {childContent.GetContent()}
                             </div>";

            output.Content.AppendHtml(content);
        }
    }
}
