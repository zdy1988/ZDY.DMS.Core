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
    [HtmlTargetElement("tree-table-template-column", ParentTag = "tree-table")]
    public class TreeTableTemplateColumnTagHelper : HelperBase
    {
        public virtual string DisplayName { get; set; }

        public virtual string FieldName { get; set; }

        public virtual bool IsCentered { get; set; } = false;

        public virtual int Width { get; set; }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();

            if (context.TryGetContext<TreeTableContext, TreeTableTagHelper>(out TreeTableContext treeTableContext))
            {
                var childContent = await output.GetChildContentAsync();
                if (!childContent.IsEmptyOrWhiteSpace)
                {
                    treeTableContext.TreeTableTemplateColumns.Add(new Tuple<TreeTableTemplateColumnTagHelper, IHtmlContent>(this, childContent));
                }
            }
        }
    }
}
