using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.DMS.AspNetCore.Auth;

namespace ZDY.DMS.Web.Pages.Permission
{
    //[Authorize]
    public class UserGroupManagerModel : PageModel
    {
        public Guid CompanyId { get; set; }

        public void OnGet(Guid companyId)
        {
            if (companyId.Equals(default))
            {
                //var userIdentity = this.HttpContext.GetUserIdentity();
                //companyId = userIdentity.CompanyId;

                companyId = Guid.Empty;
            }

            CompanyId = companyId;
        }
    }
}