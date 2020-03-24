using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZDY.DMS.AspNetCore.Auth;
using ZDY.DMS.Metronic.TagHelpers.Tree;
using ZDY.DMS.Repositories;

namespace ZDY.DMS.Web.Pages.Admin
{
    public class PageTreeModel : PageModel
    {
        private readonly IRepositoryContext repositoryContext;
        private readonly IRepository<Guid, Models.Page> pageRepository;

        public PageTreeModel(IRepositoryContext repositoryContext)
        {
            this.repositoryContext = repositoryContext;
            this.pageRepository = repositoryContext.GetRepository<Guid, Models.Page>();
        }

        public List<TreeNode> TreeData { get; set; }

        public void OnGet()
        {
            var userIdentity = this.HttpContext.GetUserIdentity();

            var companies = new Guid[] { default, userIdentity.CompanyId };

            if (userIdentity.IsAdministrator)
            {
                TreeData = pageRepository
                    .FindAll(t => companies.Contains(t.CompanyId))
                    .Select(t => new TreeNode
                    {
                        Data = t,
                        Id = t.Id,
                        Name = t.PageName,
                        Order = t.Order,
                        ParentId = t.ParentId
                    }).ToList();
            }
            else
            {
                TreeData = pageRepository
                    .FindAll(t => companies.Contains(t.CompanyId))
                    .Select(t => new TreeNode
                    {
                        Data = t,
                        Id = t.Id,
                        Name = t.PageName,
                        Order = t.Order,
                        ParentId = t.ParentId
                    }).ToList();
            }
        }
    }
}