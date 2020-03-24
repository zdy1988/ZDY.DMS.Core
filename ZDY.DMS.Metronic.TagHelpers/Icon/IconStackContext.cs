using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.Metronic.TagHelpers
{
    public class IconStackContext
    {
        public List<TagBuilder> Icons { get; set; } = new List<TagBuilder>();
    }
}
