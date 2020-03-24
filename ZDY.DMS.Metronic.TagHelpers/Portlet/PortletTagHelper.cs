using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.Metronic.TagHelpers.Untils;

namespace ZDY.DMS.Metronic.TagHelpers.Portlet
{
    [HtmlTargetElement("portlet")]
    public class PortletTagHelper : TagHelper
    {
        public string PortletClass { get; set; } = "";

        public string Title { get; set; } = "Default Title";

        public string TitleClass { get; set; } = "uppercase";

        public Icon TitleIcon { get; set; } = Icon.Slack;

        public string TitleIconClass { get; set; } = "";

        public string Mode { get; set; } = "light";

        public bool IsFit { get; set; } = false;

        public bool IsForm { get; set; } = false;

        public bool IsShowTools { get; set; } = true;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var portletContext = new PortletContext();
            context.Items.Add(typeof(PortletTagHelper), portletContext);

            var childContent = await output.GetChildContentAsync();

            var portletClasses = CssClassBuilder.Build(
                new CssClassName("portlet", true),
                new CssClassName(Mode.ToLower(), true),
                new CssClassName("portlet-fit", IsFit),
                new CssClassName("portlet-form", IsForm),
                new CssClassName(PortletClass, !string.IsNullOrEmpty(PortletClass)));

            var titleIconClasses = CssClassBuilder.Build(
                new CssClassName($"fa fa-{TitleIcon.ToValue()}", TitleIcon != Icon.None),
                new CssClassName(TitleIconClass, !string.IsNullOrEmpty(TitleIconClass)));

            var iconContent = TitleIcon != Icon.None ? $"<i class='{titleIconClasses}'></i>" : "";

            output.TagName = "div";

            output.Attributes.Add("class", portletClasses);

            var tools = $@"<div class='tools'>
                              <a href='javascript:;' class='collapse'> </a>
                              <a href='javascript:;' class='reload' "+ " data-bind='{click:refresh}'" + @"> </a>
                              <a href='javascript:;' class='fullscreen'> </a>
                              <a href='javascript:;' class='remove'> </a>
                          </div>";

            var content = $@"<div class='portlet-title'>
                                 <div class='caption'>
                                     {iconContent}
                                     <span class='caption-subject {TitleClass}'>{Title}</span>
                                 </div>
                                 {(portletContext.Actions != null ? portletContext.Actions.ToHtml() : "")}
                                 {(IsShowTools ? tools : "")}
                             </div>
                             <div class='portlet-body'>{childContent.GetContent()}</div>";

            output.Content.SetHtmlContent(content);
        }
    }
}
