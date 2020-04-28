using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ZDY.Metronic.UI.Untils;
using Newtonsoft.Json;

namespace ZDY.Metronic.UI.TagHelpers
{
    [HtmlTargetElement("tree-table")]
    public class TreeTableTagHelper : HelperBase
    {
        public bool IsHeaderDestroyed { get; set; } = false;

        public List<TreeTableField> Fields { get; set; }

        public List<TreeTableItem> Dataset { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context.TryAddContext<TreeTableContext, TreeTableTagHelper>(out TreeTableContext treeTableContext))
            {
                await output.GetChildContentAsync();

                output.TagName = "div";

                output.Attributes.Add("id", Id);

                output.Attributes.Add("class", "table-scrollable");

                var table = new TagBuilder("table");

                table.AddCssClass("table table-hover table-tree");

                if (!IsHeaderDestroyed)
                {
                    table.InnerHtml.AppendHtml(BuildTreeTableHead(treeTableContext));
                }

                table.InnerHtml.AppendHtml(BuildTreeTableBody(treeTableContext));

                output.Content.SetHtmlContent(table);

                return;
            }

            output.SuppressOutput();
        }

        internal TagBuilder BuildTreeTableHead(TreeTableContext treeTableContext)
        {
            var thead = new TagBuilder("thead");

            var row = new TagBuilder("tr");

            if (Fields?.Count() > 0)
            {
                foreach (var field in Fields)
                {
                    var cell = BuildTreeTableCell("th", field.Width, field.IsCentered);

                    cell.InnerHtml.AppendHtml(field.DisplayName);

                    row.InnerHtml.AppendHtml(cell);
                }
            }

            if (treeTableContext.TreeTableTemplateColumns?.Count > 0)
            {
                foreach (var column in treeTableContext.TreeTableTemplateColumns)
                {
                    var cell = BuildTreeTableCell("th", column.Item1.Width, column.Item1.IsCentered);

                    cell.InnerHtml.AppendHtml(column.Item1.DisplayName);

                    row.InnerHtml.AppendHtml(cell);
                }
            }

            thead.InnerHtml.AppendHtml(row);

            return thead;
        }

        internal TagBuilder BuildTreeTableBody(TreeTableContext treeTableContext)
        {
            var tbody = new TagBuilder("tbody");

            if (Fields.IsNotNull() && Fields.Any() && Dataset.IsNotNull() && Dataset.Any())
            {
                var list = ListInitialize();

                foreach (var item in list)
                {
                    var row = new TagBuilder("tr");

                    row.AddCssClass($"table-row-level-{item.Item3}");

                    row.Attributes.Add("data-parent", item.Item1.ParentId.ToString());
                    row.Attributes.Add("data-id", item.Item1.Id.ToString());
                    row.Attributes.Add("data-json", JsonConvert.SerializeObject(item.Item1.Data));

                    var type = item.Item1.Data.GetType();

                    for (var i = 0; i < Fields.Count; i++)
                    {
                        var field = Fields[i];

                        var value = type.GetProperty(field.FieldName).GetValue(item.Item1.Data, null)?.ToString();

                        var cell = BuildTreeTableCell("td", field.Width, field.IsCentered);

                        if (i == 0)
                        {
                            if (item.Item2)
                            {
                                cell.AddCssClass("table-row-slide");

                                cell.InnerHtml.AppendHtml(@"<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' width='24px' height='24px' viewBox='0 0 24 24' version='1.1' class='kt-svg-icon'>
                                                                <g stroke='none' stroke-width='1' fill='none' fill-rule='evenodd'>
                                                                    <polygon points='0 0 24 0 24 24 0 24'/>
                                                                    <path d='M8.2928955,3.20710089 C7.90237121,2.8165766 7.90237121,2.18341162 8.2928955,1.79288733 C8.6834198,1.40236304 9.31658478,1.40236304 9.70710907,1.79288733 L15.7071091,7.79288733 C16.085688,8.17146626 16.0989336,8.7810527 15.7371564,9.17571874 L10.2371564,15.1757187 C9.86396402,15.5828377 9.23139665,15.6103407 8.82427766,15.2371482 C8.41715867,14.8639558 8.38965574,14.2313885 8.76284815,13.8242695 L13.6158645,8.53006986 L8.2928955,3.20710089 Z' fill='#000000' fill-rule='nonzero' transform='translate(12.000003, 8.499997) scale(-1, -1) rotate(-90.000000) translate(-12.000003, -8.499997) '/>
                                                                    <path d='M6.70710678,19.2071045 C6.31658249,19.5976288 5.68341751,19.5976288 5.29289322,19.2071045 C4.90236893,18.8165802 4.90236893,18.1834152 5.29289322,17.7928909 L11.2928932,11.7928909 C11.6714722,11.414312 12.2810586,11.4010664 12.6757246,11.7628436 L18.6757246,17.2628436 C19.0828436,17.636036 19.1103465,18.2686034 18.7371541,18.6757223 C18.3639617,19.0828413 17.7313944,19.1103443 17.3242754,18.7371519 L12.0300757,13.8841355 L6.70710678,19.2071045 Z' fill='#000000' fill-rule='nonzero' opacity='0.3' transform='translate(12.000003, 15.499997) scale(-1, -1) rotate(-360.000000) translate(-12.000003, -15.499997) '/>
                                                                </g>
                                                            </svg>");
                            }
                            else
                            { 
                                cell.InnerHtml.AppendHtml(@"<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' width='24px' height='24px' viewBox='0 0 24 24' version='1.1' class='kt-svg-icon'>
                                                                <g stroke='none' stroke-width='1' fill='none' fill-rule='evenodd'>
                                                                    <rect x='0' y='0' width='24' height='24'/>
                                                                    <circle fill='#000000' opacity='0.3' cx='12' cy='12' r='10'/>
                                                                    <rect fill='#000000' x='6' y='11' width='12' height='2' rx='1'/>
                                                                </g>
                                                            </svg>");
                            }
                        }

                        cell.InnerHtml.AppendHtml(value);

                        row.InnerHtml.AppendHtml(cell);
                    }

                    if (treeTableContext.TreeTableTemplateColumns?.Count > 0)
                    {
                        foreach (var column in treeTableContext.TreeTableTemplateColumns)
                        {
                            var cell = BuildTreeTableCell("td", column.Item1.Width, column.Item1.IsCentered);

                            if (!String.IsNullOrWhiteSpace(column.Item1.FieldName))
                            {
                                var value = type.GetProperty(column.Item1.FieldName).GetValue(item.Item1.Data, null).ToString();

                                var html = column.Item2.ToHtml();

                                if (!String.IsNullOrWhiteSpace(html))
                                {
                                    cell.InnerHtml.AppendHtml(String.Format(html, value));
                                }
                                else
                                {
                                    cell.InnerHtml.AppendHtml(value);
                                }
                            }
                            else
                            {
                                cell.InnerHtml.AppendHtml(column.Item2);
                            }

                            row.InnerHtml.AppendHtml(cell);
                        }
                    }

                    tbody.InnerHtml.AppendHtml(row);
                }
            }

            return tbody;
        }

        internal TagBuilder BuildTreeTableCell(string tagName, int width, bool isCentered)
        {
            var cell = new TagBuilder(tagName);

            var style = StyleBuilder.Build(
                ("width", $"{width}px", width > 0),
                ("text-align", "center", isCentered));

            cell.Attributes.Add("style", style);

            return cell;
        }

        internal List<ValueTuple<TreeTableItem, bool, int>> ListInitialize()
        {
            // bool = 是否包含子节点
            // int = 子节点等级
            var sortedTableData = new List<ValueTuple<TreeTableItem, bool, int>>();

            var rootItems = FindChildItems(default);

            foreach (var item in rootItems)
            {
                ListInitialize(item, sortedTableData, 0);
            }

            return sortedTableData;
        }

        internal void ListInitialize(ValueTuple<TreeTableItem, bool, int> parent, List<ValueTuple<TreeTableItem, bool, int>> list, int level)
        {
            var items = FindChildItems(parent.Item1.Id);

            parent.Item2 = items.Count > 0;

            parent.Item3 = level;

            list.Add(parent);

            foreach (var item in items)
            {
                ListInitialize(item, list, level + 1);
            }
        }

        internal List<ValueTuple<TreeTableItem, bool, int>> FindChildItems(Guid parentId)
        {
            return Dataset.Where(t => t.ParentId.Equals(parentId))
                                    .OrderBy(t => t.Order)
                                    .Select(t => new ValueTuple<TreeTableItem, bool, int>(t, false, 0))
                                    .ToList();
        }
    }
}
