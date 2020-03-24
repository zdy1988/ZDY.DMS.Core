using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.DMS.AspNetCore.Auth;

namespace ZDY.DMS.Web.Pages.Home
{
    public class UserProfileModel : PageModel
    {
        public Guid UserId { get; set; }

        public void OnGet()
        {
            UserId = this.HttpContext.GetUserIdentity().Id;
        }
    }
}