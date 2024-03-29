﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ZDY.Metronic.UI.Untils;

namespace ZDY.Metronic.UI.TagHelpers
{
    [HtmlTargetElement("template-column", ParentTag = "data-table2", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class DataTableTemplateColumnTagHelper : HelperBase
    {
        public virtual int Index { get; set; } = -1;

        public virtual string FieldName { get; set; }

        public virtual string DisplayName { get; set; }

        public virtual bool IsCenter { get; set; }

        public virtual bool IsSort { get; set; }

        public virtual int Width { get; set; }

        public virtual bool IsAutoWidth { get; set; } = false;

        public virtual bool IsAutoHide { get; set; } = false;

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context.TryGetContext<DataTableContext, DataTable2TagHelper>(out DataTableContext dataTableContext))
            {
                ChildHtmlContent = await output.GetChildHtmlContentAsync();

                dataTableContext.TemplateColumns.Add(this);
            }

            output.SuppressOutput();
        }
    }
}
