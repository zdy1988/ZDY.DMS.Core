using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.Metronic.UI;

namespace ZDY.DMS.Web.Pages.Admin
{
    public class PageManagerModel : PageModel
    {
        private readonly IMetronic metronic;

        public PageManagerModel(IMetronic metronic)
        {
            this.metronic = metronic;
        }

        public List<SelectOption> PageKinds { get; set; }

        public List<SelectOption> IconSource { get; set; }

        public void OnGet()
        {
            PageKinds = new List<SelectOption> {
                new SelectOption{Value="P",Name= "页面" },
                new SelectOption{Value="N",Name= "节点" }
            };

            IconSource = metronic.GetIconDictionary<SvgIcon>().Select(t => new SelectOption
            {
                Name = t.Value,
                Value = t.Key.ToString()
            }).ToList();
        }
    }
}