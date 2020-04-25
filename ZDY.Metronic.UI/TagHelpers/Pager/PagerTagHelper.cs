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
    [HtmlTargetElement("pager", TagStructure= TagStructure.NormalOrSelfClosing)]
    public class PagerTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await output.GetChildContentAsync();

            output.TagName = "div";

            output.Attributes.Add("class", "kt-pagination  kt-pagination--info");

            output.Attributes.Add("data-bind", "visible:recordSet().length>0");

            output.Attributes.Add("style", "margin: 5px;");

            var content = @"<div class='kt-pagination__toolbar'>
                                <div class='pagination__desc'>
                                    显示 <span data-bind='text:$root.pageSizeStart'></span> - <span data-bind='text:$root.pageSizeEnd'></span> 条，每页 <select class='form-control kt-font-info' style='width: 60px; display: inline; margin: 0px;' data-bind='value:pageSize,event:{&quot;change&quot;:$root.firstPage}'><option value='10'>10</option><option value='20'>20</option><option value='50'>50</option><option value='100'>100</option><option value='1000000000'>全部</option></select> 条记录，共 <span data-bind='text:$root.count'></span> 条记录
                                </div>
                            </div>
                            <div class='kt-pagination__links'>
                                <ul class='pagination pagination-sm'>
                                    <!--ko if:showStartPagerDot-->
                                    <li class='kt-pagination__link--first' data-bind='click:firstPage'>
                                        <a href='javascript:;'><i class='fa fa-angle-double-left kt-font-info'></i></a>
                                    </li>
                                    <!--/ko-->
                                    <li class='kt-pagination__link--prev' data-bind='click:prevPage'>
                                        <a href='javascript:;'><i class='fa fa-angle-left kt-font-info'></i></a>
                                    </li>
                                    <!--ko foreach:pageNumbers-->
                                    <li class='' data-bind='click: $root.turnPage,css: { &quot;kt-pagination__link--active&quot;:$data == $root.pageIndex() }'>
                                        <a href='javascript:;' data-bind='text: $data'>1</a>
                                    </li>
                                    <!--/ko-->
                                    <li class='kt-pagination__link--next' data-bind='click:nextPage'>
                                        <a href='javascript:;'><i class='fa fa-angle-right kt-font-info'></i></a>
                                    </li>
                                    <!--ko if:showEndPagerDot-->
                                    <li class='kt-pagination__link--last' data-bind='click:lastPage'>
                                        <a href='javascript:;'><i class='fa fa-angle-double-right kt-font-info'></i></a>
                                    </li>
                                    <!--/ko-->
                                </ul>
                            </div>";
            output.Content.SetHtmlContent(content);
        }
    }
}
