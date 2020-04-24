using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.DMS.AspNetCore.Auth;
using ZDY.DMS.Services.AdminService.ServiceContracts;

namespace ZDY.DMS.Web.Pages.Admin
{
    public class PageTreeModel : PageModel
    {
        private readonly IPageService pageService;

        public PageTreeModel(IPageService pageService)
        {
            this.pageService = pageService;
        }

        //public List<TreeNode> TreeData { get; set; }

        public void OnGet()
        {
            var userIdentity = this.HttpContext.GetUserIdentity();

            //TreeData = pageService.GetAllPages(userIdentity.CompanyId).Select(t => new TreeNode
            //{
            //    Data = t,
            //    Id = t.Id,
            //    Name = t.PageName,
            //    Order = t.Order,
            //    ParentId = t.ParentId
            //}).ToList();
        }
    }
}