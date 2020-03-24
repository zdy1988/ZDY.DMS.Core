using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZDY.DMS.Metronic.TagHelpers.Modal
{
    [HtmlTargetElement("modal-footer", ParentTag = "modal")]
    public class ModalFooterTagHelper : TagHelper
    {
        public bool IsShowDismiss { get; set; } = true;

        public string DismissText { get; set; } = "取消";


        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();
            var footerContent = new DefaultTagHelperContent();
            if (IsShowDismiss)
            {
                footerContent.AppendFormat(@"<button type='button' class='btn btn-sm btn-default' data-dismiss='modal'>{0}</button>", DismissText);
            }
            footerContent.AppendHtml(childContent);
            var modalContext = (ModalContext)context.Items[typeof(ModalTagHelper)];
            modalContext.Footer = footerContent;
            output.SuppressOutput();
        }
    }
}
