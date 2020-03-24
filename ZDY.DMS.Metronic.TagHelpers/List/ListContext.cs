﻿using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.Metronic.TagHelpers.List
{
    public class ListContext
    {
        public List<Tuple<ListItemTagHelper, IHtmlContent>> Items { get; set; } = new List<Tuple<ListItemTagHelper, IHtmlContent>>();
    }
}
