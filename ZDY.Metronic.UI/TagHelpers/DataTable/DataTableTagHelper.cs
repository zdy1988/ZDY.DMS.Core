using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ZDY.Metronic.UI.TagHelpers
{
    [HtmlTargetElement("data-table")]
    public class DataTableTagHelper : HelperBase
    {
        public string SourceKey { get; set; } = "recordSet";

        public string CheckedKey { get; set; } = "Id";

        public bool IsShowHeader { get; set; } = true;

        public bool IsShowCheckboxColumn { get; set; } = true;

        public string RowClickFunc { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context.TryAddContext<DataTableContext, DataTableTagHelper>(out DataTableContext dataTableContext))
            {
                await output.GetChildContentAsync();

                output.TagName = "div";

                output.Attributes.Add("id", Id);

                output.Attributes.Add("class", "table-container");

                var table = $@"<table class='table table-hover'>
                                 {(IsShowHeader ? BuildDataTableHead(dataTableContext) : "")}
                                 {BuildDataTableBody(dataTableContext)}
                             </table>";

                output.Content.SetHtmlContent(table);

                return;
            }

            output.SuppressOutput();
        }

        internal string BuildDataTableHead(DataTableContext dataTableContext)
        {
            var theadCheckboxColumn = @"<th class='table-checkbox'>
                                           <a href='javascript:;' data-bind='click: $root.selectAll, text: $root.isSelectAll() == true ? &quot;取消&quot; : &quot;全选&quot;'></a>
                                        </th>";

            var headBuilder = new StringBuilder();

            headBuilder.Append($@"<thead><tr>");

            if (IsShowCheckboxColumn)
            {
                headBuilder.Append(theadCheckboxColumn);
            }

            if (dataTableContext.Columns.Any())
            {
                foreach (var column in dataTableContext.Columns)
                {
                    var theadSortedColumnBind = !string.IsNullOrWhiteSpace(column.Filed) ? "data-bind=\"click: function(){ $root.sort({field: '" + column.Filed + "'}) }, css: {sorted: '" + column.Filed + "' == $root.orderField()}\"" : "";

                    var theadWidth = column.Width == default ? "" : $"style='width: {column.Width}px;'";

                    var theadSortAscIcon = $"<i data-bind=\"visible: '{column.Filed}' == $root.orderField() && $root.isAsc()\" class='flaticon2-arrow-up'></i>";

                    var theadSortDescIcon = $"<i data-bind=\"visible: '{column.Filed}' == $root.orderField() && !$root.isAsc()\" class='flaticon2-arrow-down'></i>";

                    headBuilder.Append($@"<th class='{column.Classes}' {theadWidth}>
                                              <a href='javascript:;' {theadSortedColumnBind}>
                                                  {column.Text}
                                                  {theadSortAscIcon}
                                                  {theadSortDescIcon}
                                              </a>
                                          </th>");
                }
            }

            headBuilder.Append( $@"</tr></thead>");

            return headBuilder.ToString();
        }

        internal string BuildDataTableBody(DataTableContext dataTableContext)
        {
            var tbodyRowClickBind = !String.IsNullOrEmpty(RowClickFunc) ? $@"data-bind='click: $root.{RowClickFunc}'" : "";

            var tbodyCheckboxColumn = $@"<td class='table-checkbox'>
                                            <label class='kt-checkbox kt-checkbox--single kt-checkbox--solid'>
                                                <input type='checkbox' data-bind='checked:$root.selectData,value:{CheckedKey}'>&nbsp;<span></span>
                                            </label>
                                         </td>";
            var bodyBuilder = new StringBuilder();

            bodyBuilder.Append($@"<tbody>
                                    <!-- ko foreach: {SourceKey} -->
                                        <tr {tbodyRowClickBind}>");

            if (IsShowCheckboxColumn)
            {
                bodyBuilder.Append(tbodyCheckboxColumn);
            }

            if (dataTableContext.Columns.Any())
            {
                foreach (var column in dataTableContext.Columns)
                {
                    if (!String.IsNullOrWhiteSpace(column.Filed))
                    {
                        bodyBuilder.Append($@"<td class='{column.Classes}' data-bind='text:{column.Filed}'></td>");
                    }
                    else
                    {
                        bodyBuilder.Append($@"<td class='{column.Classes}'>{column.ChildHtmlContent}</td>");
                    } 
                }
            }

            bodyBuilder.Append ( $@"        </tr>
                                        <!-- /ko -->
                                    </tbody>");

            return bodyBuilder.ToString();
        }
    }
}
