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
    [HtmlTargetElement("column", ParentTag = "data-table", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class DataTableColumnTagHelper : HelperBase
    {
        public virtual string Text { get; set; }

        public virtual string Filed { get; set; }

        public virtual double Width { get; set; }

        public virtual Align Align { get; set; } = Align.None;

        public IHtmlContent ChildContent { get; set; }

        public virtual string Classes
        {
            get
            {
                return CssClasser.Build(
                    new AlignClass(Align),
                    new CssClass(ClassNames, !String.IsNullOrWhiteSpace(ClassNames))
                );;
            }
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            ChildContent = await output.GetChildContentAsync();

            output.SuppressOutput();

            if (context.TryGetContext<DataTableContext, DataTableTagHelper>(out DataTableContext dataTableContext))
            {
                dataTableContext.Columns.Add(this);
            }
        }
    }
}
