using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.Metronic.TagHelpers.Untils;

namespace ZDY.DMS.Metronic.TagHelpers.Label
{
    [HtmlTargetElement("lable")]
    public class LabelTagHelper: TagHelper
    {
        public Color Color { get; set; } = Color.None;
        public Theme Theme { get; set; } = Theme.Default;
        public Size Size { get; set; } = Size.Sm;
        public string ClassNames { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();

            var classes = CssClassBuilder.Build(
                new CssClassName("label", true),
                new CssClassName($"bg-{Color.ToValue()}", Color != Color.None),
                new CssClassName($"bg-font-{Color.ToValue()}", Color != Color.None),
                new CssClassName($"label-{Theme.ToValue()}", Theme != Theme.None),
                new CssClassName($"label-{Size.ToValue()}", Size != Size.None),
                new CssClassName(ClassNames, !string.IsNullOrEmpty(ClassNames)));

            output.TagName = "span";

            output.Attributes.Add("class", classes);

            output.Content.AppendHtml(childContent.GetContent());
        }
    }
}
