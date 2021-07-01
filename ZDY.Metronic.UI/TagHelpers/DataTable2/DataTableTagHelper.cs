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
    [HtmlTargetElement("data-table2", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class DataTable2TagHelper : HelperBase
    {
        public virtual Object Dataset { get; set; }

        public virtual List<DataTableField> Fields { get; set; }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context.TryAddContext<DataTableContext, DataTable2TagHelper>(out DataTableContext dataTableContext))
            {
                if (Dataset.IsNotNull() && Dataset.GetType().BaseType.IsAssignableFrom(typeof(IEnumerable<>)))
                {
                    await output.GetChildContentAsync();

                    output.TagName = "div";

                    output.TagMode = TagMode.StartTagAndEndTag;

                    output.Attributes.Add("id", Id);

                    output.Attributes.Add("class", "kt-datatable kt-datatable--default kt-datatable--brand kt-datatable--loaded");

                    var table = new TagBuilder("table");

                    table.AddCssClass("kt-datatable__table");

                    table.InnerHtml.AppendHtml(BuildDataTableHead(dataTableContext));

                    table.InnerHtml.AppendHtml(BuildDataTableBody(dataTableContext));

                    output.Content.SetHtmlContent(table);

                    return;
                }
            }

            output.SuppressOutput();
        }

        internal TagBuilder BuildDataTableHead(DataTableContext dataTableContext)
        {
            if (Fields.IsNull())
            {
                Fields = Dataset.GetType().GetProperty("Item").PropertyType.GetProperties().Select(t => new DataTableField { FieldName = t.Name }).ToList();
            }

            var thead = new TagBuilder("thead");

            thead.AddCssClass("kt-datatable__head");

            var row = BuildRow();

            for (var i = 0; i < Fields.Count; i++)
            {
                var field = Fields[i];

                AppendDataTableCell(row, dataTableContext, field, "th", "", i);
            }

            AppendTemplateCellToEnding(row, dataTableContext, "th");

            thead.InnerHtml.AppendHtml(row);

            return thead;
        }

        internal TagBuilder BuildDataTableBody(DataTableContext dataTableContext)
        {
            var tbody = new TagBuilder("tbody");

            tbody.AddCssClass("kt-datatable__body");

            if (Fields.IsNotNull() && Fields.Any())
            {
                var list = ((IEnumerable<Object>)Dataset).ToList();

                for (var j = 0; j < list.Count; j++)
                {
                    var item = list[j];

                    var row = BuildRow(j % 2 == 1 ? "kt-datatable__row--even" : "");

                    var type = item.GetType();

                    for (var i = 0; i < Fields.Count; i++)
                    {
                        var field = Fields[i];

                        var value = type.GetProperty(field.FieldName).GetValue(item, null).ToString();

                        AppendDataTableCell(row, dataTableContext, field, "td", value, i);
                    }

                    AppendTemplateCellToEnding(row, dataTableContext, "td");

                    tbody.InnerHtml.AppendHtml(row);
                }
            }

            return tbody;
        }

        internal TagBuilder BuildRow(string classes = "")
        {
            var row = new TagBuilder("tr");

            row.AddCssClass($"kt-datatable__row {classes}");

            row.Attributes.Add("style", "min-height: 52.5px;");

            return row;
        }

        internal TagBuilder BuildCell(string tagName, DataTableField field)
        {
            var cell = new TagBuilder(tagName);

            cell.AddCssClass("kt-datatable__cell");

            if (field.IsNotNull())
            {
                cell.Attributes.Add("data-field", field.FieldName);
            }

            return cell;
        }

        internal void AppendDataTableCell(TagBuilder row, DataTableContext dataTableContext, DataTableField field, string tagName, string value, int columnIndex)
        {
            var checkboxColumn = dataTableContext.CheckboxColumns.FirstOrDefault(t => t.FieldName == field.FieldName);

            var templateColumn = dataTableContext.TemplateColumns.FirstOrDefault(t => t.FieldName == field.FieldName);

            var cell = BuildCell(tagName, field);

            if (checkboxColumn.IsNotNull())
            {
                BuildDataTableCheckboxCell(cell, value);
            }
            else if (templateColumn.IsNotNull())
            {
                BuildDataTableTemplateCell(cell, templateColumn);
            }
            else
            {
                AppendTemplateCellByIndex(row, dataTableContext, field, tagName, columnIndex);

                BuildDataTableTextCell(cell, field, value);
            }

            row.InnerHtml.AppendHtml(cell);
        }

        internal void AppendTemplateCellByIndex(TagBuilder row, DataTableContext dataTableContext, DataTableField field, string tagName, int columnIndex)
        {
            var templateColumn2 = dataTableContext.TemplateColumns.FirstOrDefault(t => t.Index == columnIndex && String.IsNullOrWhiteSpace(t.FieldName));

            if (templateColumn2.IsNotNull())
            {
                var cell = BuildCell(tagName, field);

                BuildDataTableTemplateCell(cell, templateColumn2);

                row.InnerHtml.AppendHtml(cell);
            }
        }

        internal void AppendTemplateCellToEnding(TagBuilder row, DataTableContext dataTableContext, string tagName)
        {
            var templateColumns2 = dataTableContext.TemplateColumns.Where(t => t.Index < 0 && String.IsNullOrWhiteSpace(t.FieldName));

            if (templateColumns2.Any())
            {
                foreach (var templateColumn in templateColumns2)
                {
                    var cell = BuildCell(tagName, null);

                    BuildDataTableTemplateCell(cell, templateColumn);

                    row.InnerHtml.AppendHtml(cell);
                }
            }
        }

        internal void BuildDataTableTextCell(TagBuilder cell, DataTableField field, string value)
        {
            if (!field.IsAutoHide)
            {
                cell.Attributes.Add("data-autohide-disabled", "false");
            }

            if (field.IsAutoWidth)
            {
                cell.Attributes.Add("data-width", "auto");
            }
            else if (field.Width > 0 && !field.IsAutoWidth)
            {
                cell.Attributes.Add("data-width", field.Width.ToString());
            }

            var classes = CssClasser.Build(
              new CssClass("kt-datatable__cell--sort", field.IsSort),
              new CssClass("kt-datatable__cell--center", field.IsCenter)
            );

            cell.AddCssClass(classes);

            if (cell.TagName == "td")
            {
                cell.InnerHtml.AppendHtml($@"<span>{value}</span>");
            }
            else
            {
                var name = String.IsNullOrWhiteSpace(field.DisplayName) ? field.FieldName : field.DisplayName;

                cell.InnerHtml.AppendHtml($@"<span>{name}</span>");
            }
        }

        internal void BuildDataTableCheckboxCell(TagBuilder cell, string value)
        {
            cell.AddCssClass("kt-datatable__cell--center kt-datatable__cell--check");

            cell.Attributes.Add("data-width", "20");

            cell.Attributes.Add("data-autohide-disabled", "false");

            cell.InnerHtml.AppendHtml($@"<span>
                                           <label class='kt-checkbox kt-checkbox--single kt-checkbox--solid {(String.IsNullOrWhiteSpace(value) ? "kt-checkbox--all" : "")}'>
                                               <input type='checkbox' value='{value}'>
                                               &nbsp;
                                               <span></span>
                                           </label>
                                       </span>");
        }

        internal void BuildDataTableTemplateCell(TagBuilder cell, DataTableTemplateColumnTagHelper templateColumn)
        {
            if (!templateColumn.IsAutoHide)
            {
                cell.Attributes.Add("data-autohide-disabled", "false");
            }

            if (templateColumn.IsAutoWidth)
            {
                cell.Attributes.Add("data-width", "auto");
            }
            else if (templateColumn.Width > 0 && !templateColumn.IsAutoWidth)
            {
                cell.Attributes.Add("data-width", templateColumn.Width.ToString());
            }

            var classes = CssClasser.Build(
                new CssClass("kt-datatable__cell--sort", templateColumn.IsSort),
                new CssClass("kt-datatable__cell--center", templateColumn.IsCenter)
            );

            cell.AddCssClass(classes);

            if (cell.TagName == "td")
            {
                cell.InnerHtml.AppendHtml($"<span>{templateColumn.ChildHtmlContent}</span>");
            }
            else
            {
                var name = String.IsNullOrWhiteSpace(templateColumn.DisplayName) ? templateColumn.FieldName : templateColumn.DisplayName;

                cell.InnerHtml.AppendHtml($"<span>{name}</span>");
            }
        }
    }
}
