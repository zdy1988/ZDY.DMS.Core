using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZDY.DMS.Metronic.TagHelpers.Modal
{
    [HtmlTargetElement("modal-body", ParentTag = "modal")]
    public class ModalBodyTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();
            var modalContext = (ModalContext)context.Items[typeof(ModalTagHelper)];
            modalContext.Body = childContent;
            output.SuppressOutput();
        }
    }
}
