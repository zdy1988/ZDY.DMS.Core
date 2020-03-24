using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.Metronic.TagHelpers.FormBuilder;
using ZDY.DMS.Metronic.TagHelpers.Untils;

namespace ZDY.DMS.Metronic.TagHelpers.CheckBox
{
    [HtmlTargetElement("check-box")]
    public class CheckBoxTagHelper : TagHelper
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";
        public string Field { get; set; } = "";
        public State State { get; set; } = State.None;
        public CheckBoxMode Mode { get; set; } = CheckBoxMode.List;
        public bool IsRequired { get; set; } = false;
        public bool IsDisabled { get; set; } = false;
        public IEnumerable<KeyValuePair<string, string>> Options { get; set; }
        public int MinLength { get; set; } = int.MinValue;
        public int MaxLength { get; set; } = int.MaxValue;
        public string Value { get; set; } = "";
        public string Bind { get; set; }
        public string Rule { get; set; }
        public string ClassNames { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();

            var classes = CssClassBuilder.Build(
                new CssClassName("form-group form-md-checkboxes", true),
                new CssClassName($"has-{State}", State != State.None),
                new CssClassName(ClassNames, !string.IsNullOrEmpty(ClassNames)));

            output.TagName = "div";

            output.Attributes.Add("class", classes);

            var id = $"checkbox_{ID}";

            var required = IsRequired ? "<span class='required'>*</span>" : "";

            if (!string.IsNullOrEmpty(Name))
            {
                output.Content.AppendHtml($@"<label for='{id}'>
                                             {Name}
                                             {required}
                                         </label>");
            }

            var checkboxList = new TagBuilder("div");
            checkboxList.AddCssClass($"md-checkbox-{Mode.ToValue()}");

            var dataRule = DataRuleBuilder.Build(
                new DataRuleName("required", IsRequired),
                new DataRuleName($"minlength:{MinLength}", MinLength != int.MinValue),
                new DataRuleName($"minlength:{MaxLength}", MaxLength != int.MaxValue),
                new DataRuleName(Rule, !string.IsNullOrEmpty(Rule)));

            var dataBind = DataRuleBuilder.Build(
                new DataRuleName($"checked:{Field}", !string.IsNullOrEmpty(Field)),
                new DataRuleName(Bind, !string.IsNullOrEmpty(Bind)));


            if (Options == null)
            {
                Options = new List<KeyValuePair<string, string>> {
                     new KeyValuePair<string, string>(Value, "")
                };
            }

            int index = 1;
            foreach (var item in Options)
            {
                var checkboxId = $"{id}_{index}";
                checkboxList.InnerHtml.AppendHtml(
                    $@"<div class='md-checkbox'>
                           <input type='checkbox' 
                            id='{checkboxId}' 
                            name='{Field}' 
                            value='{item.Key}' 
                            {(!string.IsNullOrEmpty(dataRule) ? $"data-rule='{dataRule}'" : "")}
                            {(!string.IsNullOrEmpty(dataBind) ? $"data-bind='{dataBind}'" : "")}
                            {(IsDisabled ? "disabled" : "")} 
                            class='md-check'>
                           <label for='{checkboxId}'>
                               <span class='inc'></span>
                               <span class='check'></span>
                               <span class='box'></span> {item.Value}
                           </label>
                       </div>");

                index++;
            }

            output.Content.AppendHtml(checkboxList);
        }
    }
}
