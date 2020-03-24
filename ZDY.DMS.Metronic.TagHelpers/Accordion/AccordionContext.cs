using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.Metronic.TagHelpers.Accordion
{
    public class AccordionContext
    {
        public List<Tuple<AccordionPanelTagHelper, IHtmlContent>> AccordionPanels { get; set; } = new List<Tuple<AccordionPanelTagHelper, IHtmlContent>>();
    }
}
