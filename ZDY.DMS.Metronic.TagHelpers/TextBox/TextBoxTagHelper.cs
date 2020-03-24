using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.Metronic.TagHelpers.FormBuilder;
using ZDY.DMS.Metronic.TagHelpers.Untils;

namespace ZDY.DMS.Metronic.TagHelpers.TextBox
{
    [HtmlTargetElement("text-box")]
    public class TextBoxTagHelper : TagHelper
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";
        public string Field { get; set; } = "";
        public Icon Icon { get; set; } = Icon.None;
        public TextBoxKinds TextBoxType { get; set; } = TextBoxKinds.Text;
        public State State { get; set; } = State.None;
        public bool IsRequired { get; set; } = false;
        public bool IsDisabled { get; set; } = false;
        public bool IsMultiple { get; set; } = false;
        public int MultipleRows { get; set; } = 3;
        public bool IsDatePicker { get; set; } = false;
        public bool IsDateTimePicker { get; set; } = false;
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

            var placeholder = $"请输入{Name}...";

            var input = new TagBuilder(IsMultiple ? "textarea" : "input");

            var inputClasses = CssClassBuilder.Build(
                new CssClassName("form-control", true),
                new CssClassName("date-picker", IsDatePicker),
                new CssClassName("date-time-picker", !IsDatePicker && IsDateTimePicker));

            var dataRule = DataRuleBuilder.Build(
                new DataRuleName("required", IsRequired),
                new DataRuleName(Rule, !string.IsNullOrEmpty(Rule)));

            var dataBind = DataRuleBuilder.Build(
                new DataRuleName($"value:{Field}", !string.IsNullOrEmpty(Field)),
                new DataRuleName(Bind, !string.IsNullOrEmpty(Bind)));

            input.Attributes.Add("class", inputClasses);
            input.Attributes.Add("id", id);
            input.Attributes.Add("name", Field);
            input.Attributes.Add("placeholder", placeholder);
            if (!string.IsNullOrEmpty(dataRule))
            {
                input.Attributes.Add("data-rule", dataRule);
            }
            if (!string.IsNullOrEmpty(dataBind))
            {
                input.Attributes.Add("data-bind", dataBind);
            }
            if (IsDisabled)
            {
                input.Attributes.Add("disabled", "disabled");
            }
            if (IsMultiple)
            {
                input.Attributes.Add("rows", MultipleRows.ToString());
            }
            else
            {
                input.Attributes.Add("type", TextBoxType.ToValue());
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
                inputIcon.InnerHtml.AppendHtml(input);
                output.Content.AppendHtml(inputIcon);
            }
            else
            {
                output.Content.AppendHtml(input);
            }
        }
    }
}
