using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZDY.DMS.Metronic.TagHelpers.Pager
{
    [HtmlTargetElement("pager")]
    public class PagerTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await output.GetChildContentAsync();

            output.TagName = "div";

            output.Attributes.Add("class", "row");

            output.Attributes.Add("data-bind", "visible:recordSet().length>0");

            var content = @"<div class='col-xs-12 col-sm-6 col-md-6 col-lg-6 text-left' style='height: 48px; line-height: 48px; overflow: hidden;'>
                                <div class='pagination-info'>
                                    显示 <span data-bind='text:$root.pageSizeStart'></span> - <span data-bind='text:$root.pageSizeEnd'></span> 条，每页<select data-bind='value:pageSize,event:{&quot;change&quot;:$root.firstPage}'><option value='10'>10</option><option value='20'>20</option><option value='50'>50</option><option value='100'>100</option><option value='1000000000'>全部</option></select> 条记录，共 <span data-bind='text:$root.count'></span> 条记录
                                </div>
                            </div>
                            <div class='col-xs-12 col-sm-6 col-lg-6 text-right' style='height: 48px; line-height: 48px; overflow: hidden;'>
                                <ul class='pagination pagination-sm'>
                                    <!--ko if:showStartPagerDot-->
                                    <li class='' data-bind='click:firstPage'>
                                        <a href='javascript:;'>首页</a>
                                    </li>
                                    <!--/ko-->
                                    <li class='prev' data-bind='click:prevPage'>
                                        <a href='javascript:;'>上页</a>
                                    </li>
                                    <!--ko foreach:pageNumbers-->
                                    <li class='' data-bind='click: $root.turnPage,css: { &quot;active&quot;:$data == $root.pageIndex() }'>
                                        <a href='javascript:;' data-bind='text: $data'>1</a>
                                    </li>
                                    <!--/ko-->
                                    <li class='next' data-bind='click:nextPage'>
                                        <a href='javascript:;'>下页</a>
                                    </li>
                                    <!--ko if:showEndPagerDot-->
                                    <li class='' data-bind='click:lastPage'>
                                        <a href='javascript:;'>末页</a>
                                    </li>
                                    <!--/ko-->
                                </ul>
                            </div>";
            output.Content.SetHtmlContent(content);
        }
    }
}
