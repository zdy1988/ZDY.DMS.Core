using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ZDY.DMS.AspNetCore.Bootstrapper.Service;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.AdminService.ServiceContracts;
using ZDY.DMS.Services.AdminService.DataTransferObjects;
using ZDY.DMS.Services.AdminService.Models;

namespace ZDY.DMS.Services.AdminService.Implementation
{
    public class PageService : ServiceBase<AdminServiceModule>, IPageService
    {
        private readonly IMapper mapper;
        private readonly IRepository<Guid, Page> pageRepository;

        public PageService(IMapper mapper, Func<Type, IRepositoryContext> repositoryContextFactory)
            : base(repositoryContextFactory)
        {
            this.mapper = mapper;
            this.pageRepository = this.GetRepository<Guid, Page>();
        }

        public async Task<IEnumerable<MultiLevelPageDTO>> GetMultiLevelPagesAsync(Guid[] pageIdRanges, Guid companyId)
        {
            var companies = new Guid[] { default, companyId };

            var page1 = await pageRepository.FindAllAsync(t => t.IsDisabled == false && t.IsPassed == false && companies.Contains(t.CompanyId) && pageIdRanges.Contains(t.Id));
            var page2 = await pageRepository.FindAllAsync(t => t.IsDisabled == false && t.IsPassed == true && companies.Contains(t.CompanyId));

            var pages = new List<Page>();

            pages.AddRange(page1);
            pages.AddRange(page2);

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

        public IEnumerable<Page> GetAllPages(Guid companyId)
        {
            var companies = new Guid[] { default, companyId };

            return this.pageRepository.FindAll(t => companies.Contains(t.CompanyId));
        }
    }
}
