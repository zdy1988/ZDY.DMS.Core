using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.Metronic.TagHelpers.Modal
{
    public class ModalContext
    {
        public IHtmlContent Body { get; set; }
        public IHtmlContent Footer { get; set; }
    }
}
