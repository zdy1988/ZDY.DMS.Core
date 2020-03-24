using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZDY.DMS.Metronic.TagHelpers.DataTable
{
    [HtmlTargetElement("data-table"), RestrictChildren("td")]
    public class DataTableTagHelper : TagHelper
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();
        public string SourceKey { get; set; } = "recordSet";
        public string HeaderKey { get; set; } = "headers";
        public string CheckedKey { get; set; } = "Id";
        public bool IsShowHeader { get; set; } = true;
        public bool IsShowCheckboxColumn { get; set; } = true;
        public bool IsShowActionColumn { get; set; } = true;
        public bool IsCustomColumnWidth { get; set; } = false;
        public string RowClickFuntion { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();

            output.TagName = "div";

            output.Attributes.Add("id", ID);

            output.Attributes.Add("class", "table-scrollable");

            var customColumnWidthBind = "data-bind='style: {width: width}'";

            var checkboxColumn = @"<th class='table-checkbox'>
                                       <a href='javascript:;' data-bind='click: $root.selectAll, text: $root.isSelectAll() == true ? &quot;取消&quot; : &quot;全选&quot;'></a>
                                   </th>";

            var checkboxColumn2 = $@"<td class='table-checkbox'>
                                       <input type='checkbox' data-bind='checked:$root.selectData,value:{CheckedKey}'>
                                   </td>";

            var actionColumn = @"<th style='width:1%; text-align: center;'>
                                     操作
                                 </th>";

            var rowClickFunctionBind = $@"data-bind='click: $root.{RowClickFuntion}'";

            var header = $@"<thead>
                                <!--表头操作部分Start-->
                                <tr>
                                    {(IsShowCheckboxColumn ? checkboxColumn : "")}
                                    <!-- ko foreach: {HeaderKey} -->
                                    <th {(IsCustomColumnWidth ? customColumnWidthBind : "")}>
                                        <a href='javascript:;' data-bind='text: text, click: $root.sort'></a>
                                        <i data-bind='visible: field == $root.orderField() && $root.isAsc()' class='fa fa-chevron-up'></i>
                                        <i data-bind='visible: field == $root.orderField() && !$root.isAsc()' class='fa fa-chevron-down'></i>
                                    </th>
                                    <!-- /ko -->
                                    {(IsShowActionColumn ? actionColumn : "")}
                                </tr>
                                <!--表头操作部分End-->
                            </thead>";

            var content = $@"<table class='table table-striped table-bordered table-hover table-active'>
                                 {(IsShowHeader ? header : "")}
                                 <tbody>
                                     <!-- ko foreach: {SourceKey} -->
                                        <tr class='odd gradeX' {(!String.IsNullOrEmpty(RowClickFuntion) ? rowClickFunctionBind : "")}>
                                            {(IsShowCheckboxColumn ? checkboxColumn2 : "")}
                                            {childContent.GetContent()}
                                        </tr>
                                     <!-- /ko -->
                                 </tbody>
                             </table>";

            output.Content.SetHtmlContent(content);
        }
    }
}
