using System;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
                //var userIdentity = this.HttpContext.GetUserIdentity();
                //companyId = userIdentity.CompanyId;

                companyId = Guid.Empty;
            }

            CompanyId = companyId;
        }
    }
}