using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using ZDY.DMS.Metronic.TagHelpers.Untils;

namespace ZDY.DMS.Metronic.TagHelpers.TreeTable
{
    [HtmlTargetElement("tree-table")]
    public class TreeTableTagHelper : TagHelper
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();

        public bool IsShowHeader { get; set; } = true;

        public IEnumerable<TreeTableHead> TreeTableHeadData { get; set; }

        public IEnumerable<TreeTableItem> TreeTableBodyData { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var treeTableContext = new TreeTableContext();
            context.Items.Add(typeof(TreeTableTagHelper), treeTableContext);

            await output.GetChildContentAsync();

            output.TagName = "div";

            output.Attributes.Add("id", ID);

            output.Attributes.Add("class", "table-scrollable");

            var content = $@"<table class='table table-striped table-bordered table-hover table-active table-tree'>
                                 {(IsShowHeader ? BuildTableHeader(treeTableContext) : "")}
                                 <tbody>
                                      {BuildTableBody(treeTableContext)}
                                 </tbody>
                             </table>";

            output.Content.SetHtmlContent(content);
        }

        private string BuildTableHeader(TreeTableContext treeTableContext)
        {
            StringBuilder headerBuilder = new StringBuilder();

            headerBuilder.Append("<thead><tr>");

            if (TreeTableHeadData?.Count() > 0)
            {
                foreach (var head in TreeTableHeadData)
                {
                    headerBuilder.Append($"<th>{head.Name}</th>");
                }

                if (treeTableContext.TreeTableActions?.Count > 0)
                {
                    headerBuilder.Append($"<th style='width:1%; text-align: center;'> 操作 </th>");
                }
            }

            headerBuilder.Append("</tr></thead>");

            return headerBuilder.ToString();
        }

        private string BuildTableBody(TreeTableContext treeTableContext)
        {
            StringBuilder bodyBuilder = new StringBuilder();

            if (TreeTableBodyData.Count() > 0)
            {
                // bool = 是否包含子节点
                // int = 子集 Level
                var sortedTableData = new List<ValueTuple<TreeTableItem, bool, int>>();

                var rootTableItems = FindChildTableItems(default);

                foreach (var item in rootTableItems)
                {
                    SortTableData(item, sortedTableData, 0);
                }

                foreach (var item in sortedTableData)
                {
                    var isHasChild = item.Item2;

                    var itemSlideClass = isHasChild ? "table-tree-slide" : "";

                    var itemLevelClass = $"table-tree-item-level-{item.Item3}";

                    var dataJson = JsonConvert.SerializeObject(item.Item1.Data);

                    bodyBuilder.Append($"<tr class='odd gradeX {itemLevelClass}' data-parent='{item.Item1.ParentId}' data-id='{item.Item1.Id}' data-json='{dataJson}'>");

                    var data = item.Item1.Data;

                    var heads = TreeTableHeadData.ToList();

                    for (var i = 0; i < heads.Count; i++)
                    {
                        var head = heads[i];

                        var properties = data.GetType().GetProperties();

                        var property = properties.Where(t => t.Name == head.Field).FirstOrDefault();

                        if (property != null)
                        {
                            var value = property.GetValue(data, null);

                            bodyBuilder.Append($"<td class='{(i == 0 ? itemSlideClass : "")}'>");

                            if (i == 0)
                            {
                                if (isHasChild)
                                {
                                    bodyBuilder.Append("<i class='fa fa-chevron-down'></i>");
                                }
                                else
                                {
                                    bodyBuilder.Append("<i class='fa'></i>");
                                }
                            }

                            bodyBuilder.Append(i == 0 ? $"<b>{value}</b>" : value);
                            bodyBuilder.Append("</td>");
                        }
                    }

                    if (treeTableContext.TreeTableActions?.Count > 0)
                    {
                        for (var i = 0; i < treeTableContext.TreeTableActions.Count; i++)
                        {
                            var treeTableActionTag = treeTableContext.TreeTableActions[i].Item1;
                            var treeTableAction = treeTableContext.TreeTableActions[i].Item2;

                            bodyBuilder.Append("<td>");
                            bodyBuilder.Append(treeTableAction.ToHtml());
                            bodyBuilder.Append("</td>");
                        }
                    }

                    bodyBuilder.Append("</tr>");
                }
            }

            return bodyBuilder.ToString();
        }

        private void SortTableData(ValueTuple<TreeTableItem, bool, int> parentItem, List<ValueTuple<TreeTableItem, bool, int>> list,int level)
        {
            var items = FindChildTableItems(parentItem.Item1.Id);

            parentItem.Item2 = items.Count > 0;

            parentItem.Item3 = level;

            list.Add(parentItem);

            foreach (var item in items)
            {
                SortTableData(item, list, level + 1);
            }
        }

        private List<ValueTuple<TreeTableItem, bool, int>> FindChildTableItems(Guid parentId)
        {
            return TreeTableBodyData.Where(t => t.ParentId.Equals(parentId))
                                    .OrderBy(t => t.Order)
                                    .Select(t => new ValueTuple<TreeTableItem, bool, int>(t, false, 0))
                                    .ToList();
        }
    }
}
