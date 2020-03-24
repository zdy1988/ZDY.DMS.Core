using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Threading.Tasks;
using ZDY.DMS.Metronic.TagHelpers.Untils;

namespace ZDY.DMS.Metronic.TagHelpers.Modal
{
    [HtmlTargetElement("modal"), RestrictChildren("modal-body", "modal-footer")]
    public class ModalTagHelper : TagHelper
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();

        public string Title { get; set; }

        public ModalSize Size { get; set; } = ModalSize.None;

        public string ClassNames { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var modalContext = new ModalContext();
            context.Items.Add(typeof(ModalTagHelper), modalContext);

            await output.GetChildContentAsync();

            var classes = CssClassBuilder.Build(
                new CssClassName("modal fade", true),
                new CssClassName(ClassNames, !string.IsNullOrEmpty(ClassNames)));

            output.TagName = "div";
            output.Attributes.Add("class", classes);
            output.Attributes.Add("role", "dialog");
            output.Attributes.Add("id", ID);
            output.Attributes.Add("aria-hidden", "true");
            output.Attributes.Add("aria-labelledby", $"{context.UniqueId}Label");
            output.Attributes.Add("tabindex", "-1");

            var dialogClasses = CssClassBuilder.Build(
                     new CssClassName("modal-dialog", true),
                     new CssClassName($"modal-{Size.ToValue()}", Size != ModalSize.None)
                );

            var content =
                $@"<div class='{dialogClasses}'>
                    <div class='modal-content'>
                      <div class='modal-header'>
                        <button type = 'button' class='close' data-dismiss='modal' aria-label='Close'><span aria-hidden='true'>&times;</span></button>
                        <h4 class='modal-title' id='{context.UniqueId}Label'>{Title}</h4>
                      </div>
                        <div class='modal-body'>";

            output.Content.AppendHtml(content);
            if (modalContext.Body != null)
            {
                output.Content.AppendHtml(modalContext.Body);
            }
            output.Content.AppendHtml("</div>");
            if (modalContext.Footer != null)
            {
                output.Content.AppendHtml("<div class='modal-footer'>");
                output.Content.AppendHtml(modalContext.Footer);
                output.Content.AppendHtml("</div>");
            }

            output.Content.AppendHtml("</div></div>");
        }
    }
}
