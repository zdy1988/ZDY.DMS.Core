using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZDY.Metronic.UI.Untils;

namespace ZDY.Metronic.UI.TagHelpers
{
    [HtmlTargetElement("scroll-viewer")]
    public class ScrollViewerTagHelper  : HelperBase
    {
        public ScrollViewerMode Mode { get; set; } = ScrollViewerMode.Vertical;

        public double Height { get; set; } = 100;

        protected virtual string Classes
        {
            get
            {
                return CssClasser.Build(
                    new CssClass("scroll-viewer", true),
                    new CssClass(ClassNames, !String.IsNullOrWhiteSpace(ClassNames))
                );
            }
        }

        protected virtual string Styles
        {
            get
            {
                return Styler.Build(
                    ("height", $"{Height}px", Height != 0),
                    ("overflow", "auto", Mode == ScrollViewerMode.Both),
                    ("overflow-y", "auto", Mode == ScrollViewerMode.Vertical),
                    ("overflow-x", "auto", Mode == ScrollViewerMode.Horizontal)
                );
            }
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();

            output.TagName = "div";

            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Add("id", Id);

            output.Attributes.Add("class", Classes);

            var style = context.GetHtmlStyle();

            output.Attributes.SetAttribute("style", Styles.Append(style));

            if (!childContent.IsEmptyOrWhiteSpace)
            {
                output.Content.AppendHtml(childContent.GetContent());
            }
        }
    }
}
