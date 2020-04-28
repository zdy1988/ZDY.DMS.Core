using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.DMS.AspNetCore.Auth;

namespace ZDY.DMS.Web.Pages.Organization
{
    //[Authorize]
    public class DepartmentManagerModel : PageModel
    {
        public Guid CompanyId { get; set; }

        public void OnGet(Guid companyId)
        {
            if (companyId.Equals(default))
            {
                var identity = this.HttpContext.GetUserIdentity();
                companyId = identity.CompanyId;
            }

            CompanyId = companyId;
        }
    }
}