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
    [HtmlTargetElement("data-table")]
    public class DataTableTagHelper : HelperBase
    {
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

            output.Attributes.Add("id", Id);

            output.Attributes.Add("class", "table-scrollable");

            var customColumnWidthBind = "data-bind='style: {width: width}'";

            var checkboxColumn = @"<th class='table-checkbox'>
                                       <a href='javascript:;' data-bind='click: $root.selectAll, text: $root.isSelectAll() == true ? &quot;取消&quot; : &quot;全选&quot;'></a>
                                   </th>";

            var checkboxColumn2 = $@"<td class='table-checkbox'>
                                       <label class='kt-checkbox kt-checkbox--single kt-checkbox--solid'>
                                           <input type='checkbox' data-bind='checked:$root.selectData,value:{CheckedKey}'>&nbsp;<span></span>
                                       </label>
                                   </td>";

            var actionColumn = @"<th class='kt-align-center' style='width:1%;'>
                                    <a href='javascript:;'>操作</a>
                                 </th>";

            var rowClickFunctionBind = $@"data-bind='click: $root.{RowClickFuntion}'";

            var sortedColumnBind = "data-bind='click: $root.sort, css: {sorted: field == $root.orderField()}'";

            var header = $@"<thead>
                                <!--表头操作部分Start-->
                                <tr>
                                    {(IsShowCheckboxColumn ? checkboxColumn : "")}
                                    <!-- ko foreach: {HeaderKey} -->
                                    <th {(IsCustomColumnWidth ? customColumnWidthBind : "")}>
                                        <a href='javascript:;' {sortedColumnBind}>
                                            <!-- ko text: text --><!-- /ko -->
                                            <i data-bind='visible: field == $root.orderField() && $root.isAsc()' class='flaticon2-arrow-up'></i>
                                            <i data-bind='visible: field == $root.orderField() && !$root.isAsc()' class='flaticon2-arrow-down'></i>
                                        </a>
                                    </th>
                                    <!-- /ko -->
                                    {(IsShowActionColumn ? actionColumn : "")}
                                </tr>
                                <!--表头操作部分End-->
                            </thead>";

            var content = $@"<table class='table table-hover'>
                                 {(IsShowHeader ? header : "")}
                                 <tbody>
                                     <!-- ko foreach: {SourceKey} -->
                                        <tr {(!String.IsNullOrEmpty(RowClickFuntion) ? rowClickFunctionBind : "")}>
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
