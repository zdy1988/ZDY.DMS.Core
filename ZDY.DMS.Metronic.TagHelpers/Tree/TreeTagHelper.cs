using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZDY.DMS.Metronic.TagHelpers.Tree
{
    [HtmlTargetElement("tree")]
    public class TreeTagHelper : TagHelper
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();
        public IEnumerable<TreeNode> TreeData { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await output.GetChildContentAsync();

            output.TagName = "div";

            output.Attributes.Add("id", ID);

            TagBuilder tree = BuildTreeNode(default);

            output.Content.AppendHtml(tree);
        }

        private TagBuilder BuildTreeNode(Guid parentId)
        {
            var items = TreeData.Where(t => t.ParentId.Equals(parentId)).OrderBy(t => t.Order).ToList();

            if (items.Count() > 0)
            {
                TagBuilder ul = new TagBuilder("ul");
                foreach (var item in items)
                {
                    TagBuilder li = new TagBuilder("li");
                    li.InnerHtml.Append(item.Name);
                    li.Attributes.Add("data-json", JsonConvert.SerializeObject(item.Data));

                    var childNode = BuildTreeNode(item.Id);

                    if (childNode != null)
                    {
                        li.InnerHtml.AppendHtml(childNode);
                    }

                    ul.InnerHtml.AppendHtml(li);
                }
                return ul;
            }
            else
            {
                return null;
            }
        }
    }
}
