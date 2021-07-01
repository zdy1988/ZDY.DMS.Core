using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ZDY.Metronic.UI
{
    internal static class HelperOutputExtension
    {
        internal static void TransformOutput(this TagHelperOutput output, TagBuilder builder)
        {
            output.TagName = builder.TagName;

            output.TagMode = TagMode.StartTagAndEndTag;

            foreach (var attr in builder.Attributes)
            {
                output.Attributes.Add(attr.Key, attr.Value);
            }

            output.Content.SetHtmlContent(builder.InnerHtml);
        }

        internal static async Task<string> GetChildHtmlContentAsync(this TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();

            return childContent.IsEmptyOrWhiteSpace ? string.Empty : childContent.GetContent();
        }
    }
}
