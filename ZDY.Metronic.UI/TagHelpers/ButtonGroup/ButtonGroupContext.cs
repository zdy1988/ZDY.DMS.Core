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
    internal class ButtonGroupContext: IHelperContext
    {
        internal List<IHtmlContent> ButtonOrDropdowns { get; set; } = new List<IHtmlContent>();
    }
}
