using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.Metronic.TagHelpers.CheckBox;
using ZDY.DMS.Metronic.TagHelpers.FormBuilder;
using ZDY.DMS.Metronic.TagHelpers.Untils;

namespace ZDY.DMS.Metronic.TagHelpers.RadioBox
{
    [HtmlTargetElement("radio-box")]
    public class RadioBoxTagHelper : TagHelper
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";
        public string Field { get; set; } = "";
        public State State { get; set; } = State.None;
        public CheckBoxMode Mode { get; set; } = CheckBoxMode.List;
        public bool IsRequired { get; set; } = false;
        public bool IsDisabled { get; set; } = false;
        public IEnumerable<KeyValuePair<string, string>> Options { get; set; }
        public string Value { get; set; }
        public string Bind { get; set; }
        public string Rule { get; set; }
        public string ClassNames { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();

            var classes = CssClassBuilder.Build(
                new CssClassName("form-group form-md-radios", true),
                new CssClassName($"has-{State}", State != State.None),
                new CssClassName(ClassNames, !string.IsNullOrEmpty(ClassNames)));

            output.TagName = "div";

            output.Attributes.Add("class", classes);

            var id = $"radiobox_{ID}";

            var required = IsRequired ? "<span class='required'>*</span>" : "";

            output.Content.AppendHtml($@"<label for='{id}'>
                                             {Name}
                                             {required}
                                         </label>");

            var radioboxList = new TagBuilder("div");
            radioboxList.AddCssClass($"md-radio-{Mode.ToValue()}");

            var dataRule = DataRuleBuilder.Build(
                new DataRuleName("required", IsRequired),
                new DataRuleName(Rule, !string.IsNullOrEmpty(Rule)));

            var dataBind = DataRuleBuilder.Build(
                new DataRuleName($"checked:{Field}", !string.IsNullOrEmpty(Field)),
                new DataRuleName(Bind, !string.IsNullOrEmpty(Bind)));

            if (Options == null)
            {
                throw new ArgumentNullException("Options");
            }

            int index = 1;
            foreach (var item in Options)
            {
                var radioboxId = $"{id}_{index}";
                var radioValue = item.Key.ToString();
                try
                {
                    Convert.ToInt32(radioValue);
                }
                catch
                {
                    radioValue = $"\'{item.Key}\'";
                }
                radioboxList.InnerHtml.AppendHtml(
                    $@"<div class='md-radio'>
                           <input type='radio' id='{radioboxId}' 
                                               name='{Field}' 
                                               {(!string.IsNullOrEmpty(dataRule) ? $"data-rule='{dataRule}'" : "")}
                                               {(!string.IsNullOrEmpty(dataBind) ? $"data-bind=\"{dataBind},checkedValue:{radioValue}\"" : "")}
                                               {(IsDisabled ? "disabled" : "")} 
                                               class='md-radiobtn'>
                           <label for='{radioboxId}'>
                               <span class='inc'></span>
                               <span class='check'></span>
                               <span class='box'></span> {item.Value}
                           </label>
                       </div>");
                index++;
            }

            output.Content.AppendHtml(radioboxList);
        }
    }
}
