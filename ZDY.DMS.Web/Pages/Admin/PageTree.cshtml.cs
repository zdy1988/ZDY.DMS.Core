using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.DMS.AspNetCore.Auth;
using ZDY.DMS.Services.AdminService.ServiceContracts;
using ZDY.Metronic.UI;

namespace ZDY.DMS.Web.Pages.Admin
{
    public class PageTreeModel : PageModel
    {
        private readonly IPageService pageService;

        public PageTreeModel(IPageService pageService)
        {
            this.pageService = pageService;
        }

        public List<TreeTableField> Fields { get; set; }

        public List<TreeTableItem> Pages { get; set; }

        public async Task OnGetAsync()
        {
            var identity = this.HttpContext.GetUserIdentity();

            Fields = new List<TreeTableField> {
                new TreeTableField{ DisplayName = "页面名称", FieldName ="PageName" },
                new TreeTableField{ DisplayName = "菜单名称", FieldName ="MenuName" },
                new TreeTableField{ DisplayName = "页面路径", FieldName ="Src" }
            };

            Pages = (await pageService.GetAllPagesAsync(identity.CompanyId)).Select(t => new TreeTableItem
            {
                Id = t.Id,
                ParentId = t.ParentId,
                Data = t,
                Order = t.Order
            }).ToList();
        }
    }
}