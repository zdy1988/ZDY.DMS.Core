using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ZDY.Metronic.UI.Untils;

namespace ZDY.Metronic.UI.TagHelpers
{
    [HtmlTargetElement("cell", ParentTag = "data-table", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class DataTableCellTagHelper : HelperBase
    {
        public virtual string Filed { get; set; }

        public virtual Align Align { get; set; } = Align.None;

        protected virtual string Classes
        {
            get
            {
                return CssClassBuilder.Build(
                    new AlignClass(Align),
                    new CssClass(ClassNames, !String.IsNullOrWhiteSpace(ClassNames))
                );;
            }
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "td";

            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Add("class", Classes);

            if (!String.IsNullOrWhiteSpace(Filed))
            {
                output.Attributes.Add("data-bind", $"text:{Filed}");
            }
            else 
            {
                var childContent = await output.GetChildContentAsync();

                output.Content.SetHtmlContent(childContent);
            }
        }
    }
}
