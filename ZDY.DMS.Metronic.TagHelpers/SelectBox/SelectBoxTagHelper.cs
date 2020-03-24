using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.Metronic.TagHelpers.FormBuilder;
using ZDY.DMS.Metronic.TagHelpers.TextBox;
using ZDY.DMS.Metronic.TagHelpers.Untils;

namespace ZDY.DMS.Metronic.TagHelpers.SelectBox
{
    [HtmlTargetElement("select-box")]
    public class SelectBoxTagHelper : TagHelper
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";
        public string Field { get; set; } = "";
        public Icon Icon { get; set; } = Icon.None;
        public State State { get; set; } = State.None;
        public bool IsRequired { get; set; } = false;
        public bool IsDisabled { get; set; } = false;
        public bool IsMultiple { get; set; } = false;
        public bool IsUsePlaceHolder { get; set; } = true;
        public IEnumerable<KeyValuePair<string, string>> Options { get; set; }
        public string Bind { get; set; }
        public string Rule { get; set; }
        public string ClassNames { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await output.GetChildContentAsync();

            var classes = CssClassBuilder.Build(
                new CssClassName("form-group", true),
                new CssClassName($"has-{State}", State != State.None),
                new CssClassName(ClassNames, !string.IsNullOrEmpty(ClassNames)));

            output.TagName = "div";

            output.Attributes.Add("class", classes);

            var id = ID;

            var required = IsRequired ? "<span class='required'>*</span>" : "";

            var select = new TagBuilder("select");

            var dataRule = DataRuleBuilder.Build(
                new DataRuleName("required", IsRequired),
                new DataRuleName(Rule, !string.IsNullOrEmpty(Rule)));

            var dataBind = DataRuleBuilder.Build(
                new DataRuleName(IsMultiple ? $"selectedOptions:{Field}" : $"value:{Field}", !string.IsNullOrEmpty(Field)),
                new DataRuleName(Bind, !string.IsNullOrEmpty(Bind)));

            select.AddCssClass("form-control");
            if (IsMultiple)
            {
                select.Attributes.Add("multiple", "multiple");
            }
            select.Attributes.Add("id", id);
            select.Attributes.Add("name", Field);
            if (IsUsePlaceHolder)
            {
                var placeholder = $"请选择{Name}...";
                select.InnerHtml.AppendHtml($"<option value=''>{placeholder}</option>");
            }
            if (!string.IsNullOrEmpty(dataRule))
            {
                select.Attributes.Add("data-rule", dataRule);
            }
            if (!string.IsNullOrEmpty(dataBind))
            {
                select.Attributes.Add("data-bind", dataBind);
            }
            if (IsDisabled)
            {
                select.Attributes.Add("disabled", "disabled");
            }

            if (Options == null)
            {
                throw new ArgumentNullException("Options");
            }

            TagBuilder optgroup = null;

            foreach (var item in Options)
            {
                if (item.Key.StartsWith("@"))
                {
                    optgroup = new TagBuilder("optgroup");
                    optgroup.Attributes.Add("label", item.Value);
                    select.InnerHtml.AppendHtml(optgroup);
                }
                else
                {
                    if (optgroup != null)
                    {
                        optgroup.InnerHtml.AppendHtml($"<option value='{item.Key}'>{item.Value}</option>");
                    }
                    else
                    {
                        select.InnerHtml.AppendHtml($"<option value='{item.Key}'>{item.Value}</option>");
                    }
                }
            }

            output.Content.AppendHtml($@"<label for='{id}'>
                                            {Name}
                                            {required}
                                         </label >");

            if (Icon != Icon.None)
            {
                TagBuilder inputIcon = new TagBuilder("div");
                inputIcon.AddCssClass("input-icon");
                inputIcon.InnerHtml.AppendHtml($"<i class='fa fa-{Icon.ToValue()}'></i>");
                inputIcon.InnerHtml.AppendHtml(select);
                output.Content.AppendHtml(inputIcon);
            }
            else
            {
                output.Content.AppendHtml(select);
            }
        }
    }
}
