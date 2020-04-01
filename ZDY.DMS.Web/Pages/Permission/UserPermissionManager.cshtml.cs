using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.DMS.AspNetCore.Auth;

namespace ZDY.DMS.Web.Pages.Permission
{
    //[Authorize]
    public class UserPermissionManagerModel : PageModel
    {
        public Guid CompanyId { get; set; }

        public void OnGet(Guid companyId)
        {
            if (companyId.Equals(default))
            {
                var userIdentity = this.HttpContext.GetUserIdentity();
                companyId = userIdentity.CompanyId;
            }

            CompanyId = companyId;
        }
    }
}