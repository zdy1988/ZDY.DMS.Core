using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ZDY.DMS.Models;
using ZDY.DMS.DataTransferObjects;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.AdminService.ServiceContracts;

namespace ZDY.DMS.Services.AdminService.Implementation
{
    public class PageService : IPageService
    {
        private readonly IMapper mapper;
        private readonly IRepositoryContext repositoryContext;
        private readonly IRepository<Guid, Page> pageRepository;

        public PageService(IMapper mapper,IRepositoryContext repositoryContext)
        {
            this.mapper = mapper;
            this.repositoryContext = repositoryContext;
            this.pageRepository = repositoryContext.GetRepository<Guid, Page>();
        }

        public async Task<IEnumerable<MultiLevelPageDTO>> GetMultiLevelPagesAsync(Guid userId, Guid companyId)
        {
            var companies = new Guid[] { default, companyId };

            var context = (DbContext)repositoryContext.Session;

            var pages = await (from p in context.Set<Page>()
                               join pp in context.Set<UserGroupPagePermission>() on p.Id equals pp.PageId
                               join ug in context.Set<UserGroup>() on pp.GroupId equals ug.Id
                               join gm in context.Set<UserGroupMember>() on ug.Id equals gm.GroupId
                               join u in context.Set<User>() on gm.UserId equals u.Id
                               where u.IsDisabled == false && u.Id == userId && p.IsPassed == false && companies.Contains(p.CompanyId)
                               select p).ToListAsync();

            var pages2 = await pageRepository.FindAllAsync(p => p.IsDisabled == false && p.IsPassed == true && companies.Contains(p.CompanyId), "Order", true);

            pages.AddRange(pages2);

            var levelPages = this.mapper.Map<List<Page>, List<MultiLevelPageDTO>>(pages.Distinct().ToList());

            return GetChildLevelPages(levelPages.OrderBy(t => t.Order).ToList(), default);
        }

        public async Task<IEnumerable<MultiLevelPageDTO>> GetMultiLevelPagesAsync()
        {
            return await GetChildLevelPagesAsync(default);
        }

        public async Task<IEnumerable<MultiLevelPageDTO>> GetChildLevelPagesAsync(Guid parentId)
        {
            var pages = await pageRepository.FindAllAsync(t => t.ParentId == parentId, "Order", true);

            var levelPages = this.mapper.Map<List<Page>, List<MultiLevelPageDTO>>(pages.ToList());

            foreach (var page in levelPages)
            {
                page.ChildLevelPages = await GetChildLevelPagesAsync(page.Id);
            }

            return levelPages;
        }

        private IEnumerable<MultiLevelPageDTO> GetChildLevelPages(List<MultiLevelPageDTO> pages, Guid parentId)
        {
            var childPages = pages.Where(t => t.ParentId == parentId).ToList();
            foreach (var page in childPages)
            {
                page.ChildLevelPages = GetChildLevelPages(pages, page.Id);
            }
            return childPages;
        }
    }
}
