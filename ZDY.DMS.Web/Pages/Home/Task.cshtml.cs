using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ZDY.DMS.Web.Pages.Home
{
    public class TaskModel : PageModel
    {
        public IEnumerable<KeyValuePair<string, string>> Tags { get; set; }

        public void OnGet()
        {
            Tags = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("Pending", "Pending"),
                new KeyValuePair<string, string>("Completed", "Completed"),
                new KeyValuePair<string, string>("Testing", "Testing"),
                new KeyValuePair<string, string>("Approed", "Approed"),
                new KeyValuePair<string, string>("Rejected", "Rejected")
            };
        }
    }
}