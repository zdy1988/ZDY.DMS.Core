using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.DMS.AspNetCore.Auth;
using ZDY.DMS.Services.AdminService.ServiceContracts;
using ZDY.Metronic.UI;
using Newtonsoft.Json;

namespace ZDY.DMS.Web.Pages.Admin
{
    public class PageTreeModel : PageModel
    {
        private readonly IPageService pageService;

        public PageTreeModel(IPageService pageService)
        {
            this.pageService = pageService;
        }

        public List<NavigationItem> Navs { get; set; }

        public void OnGet()
        {
            var identity = this.HttpContext.GetUserIdentity();

            var pages = pageService.GetAllPages(identity.CompanyId).ToList();

            Navs = BuildNavs(pages, default);
        }

        public List<NavigationItem> BuildNavs(List<Services.AdminService.Models.Page> pages, Guid parentId)
        {
            var childs = pages.Where(t => t.ParentId == parentId).OrderBy(t => t.Order);

            List<NavigationItem> navs = null;

            if (childs.Count() > 0)
            {
                navs = new List<NavigationItem>();

                foreach (var child in childs)
                {
                    navs.Add(new NavigationItem
                    {
                        Text = child.MenuName,
                        JsonData = JsonConvert.SerializeObject(child),
                        Navs = BuildNavs(pages, child.Id)
                    });
                }
            }

            return navs;
        }
    }
}