using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.Metronic.UI;

namespace ZDY.DMS.Web.Pages.Home
{
    public class TaskModel : PageModel
    {
        public List<SelectOption> Tags { get; set; }

        public void OnGet()
        {
            Tags = new List<SelectOption> {
                new SelectOption{Name="Pending",Value= "Pending" },
                new SelectOption{Name="Completed", Value="Completed"},
                new SelectOption{Name="Testing",Value= "Testing"},
                new SelectOption{Name="Approed",Value= "Approed"},
                new SelectOption{Name="Rejected",Value= "Rejected"}
            };
        }
    }
}