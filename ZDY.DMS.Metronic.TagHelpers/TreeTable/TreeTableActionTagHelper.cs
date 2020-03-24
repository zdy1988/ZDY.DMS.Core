using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZDY.DMS.Metronic.TagHelpers.TreeTable
{
    [HtmlTargetElement("tree-table-action", ParentTag = "tree-table")]
    public class TreeTableActionTagHelper : TagHelper
    {
        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var treeTableContext = (TreeTableContext)context.Items[typeof(TreeTableTagHelper)];
            var childContent = await output.GetChildContentAsync();
            treeTableContext.TreeTableActions.Add(new Tuple<TreeTableActionTagHelper, IHtmlContent>(this, childContent));
            output.SuppressOutput();
        }
    }
}
