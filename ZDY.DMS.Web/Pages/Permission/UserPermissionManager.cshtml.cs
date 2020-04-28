using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.DMS.AspNetCore.Auth;
using ZDY.DMS.Services.PermissionService.Models;
using ZDY.DMS.Services.PermissionService.ServiceContracts;
using ZDY.DMS.Services.AdminService.ServiceContracts;
using ZDY.Metronic.UI;

namespace ZDY.DMS.Web.Pages.Permission
{
    //[Authorize]
    public class UserPermissionManagerModel : PageModel
    {
        private readonly IPageService pageService;
        private readonly IUserGroupService userGroupService;

        public UserPermissionManagerModel(IUserGroupService userGroupService, IPageService pageService)
        {
            this.pageService = pageService;
            this.userGroupService = userGroupService;
        }

        public List<TreeTableField> Fields { get; set; }

        public List<TreeTableItem> Pages { get; set; }

        public List<UserGroup> UserGroups { get; set; }

        public async Task OnGetAsync()
        {
            var identity = this.HttpContext.GetUserIdentity();

            Fields = new List<TreeTableField> {
                new TreeTableField{ DisplayName = "页面名称", FieldName ="PageName" }
            };

            Pages = (await pageService.GetAllPagesAsync(identity.CompanyId)).Select(t => new TreeTableItem
            {
                Id = t.Id,
                ParentId = t.ParentId,
                Data = t,
                Order = t.Order
            }).ToList();

            UserGroups = (await this.userGroupService.GetAllUserGroupAsync(identity.CompanyId)).ToList();
        }
    }
}