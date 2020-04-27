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

        public PageService(IMapper mapper)
        {
            this.mapper = mapper;
            this.pageRepository = this.GetRepository<Guid, Page>();
        }

        public async Task<IEnumerable<MultiLevelPageDTO>> GetMultiLevelPagesAsync(Guid[] pageIdRanges, Guid companyId)
        {
            var companies = new Guid[] { default, companyId };

            var pages = await pageRepository.FindAllAsync(t => t.IsDisabled == false && t.IsPermissionRequired == true && companies.Contains(t.CompanyId) && pageIdRanges.Contains(t.Id));

            var pages2 = await pageRepository.FindAllAsync(t => t.IsDisabled == false && t.IsPermissionRequired == false && companies.Contains(t.CompanyId));

            var levelPages = this.mapper.Map<IEnumerable<Page>, IEnumerable<MultiLevelPageDTO>>(pages.Union(pages2).OrderBy(t => t.Order));

            return GetChildLevelPages(levelPages, default);
        }

        public async Task<IEnumerable<MultiLevelPageDTO>> GetMultiLevelPagesAsync()
        {
            return await GetChildLevelPagesAsync(default);
        }

        public async Task<IEnumerable<MultiLevelPageDTO>> GetChildLevelPagesAsync(Guid parentId)
        {
            var pages = await pageRepository.FindAllAsync(t => t.ParentId == parentId, "Order", true);

            var levelPages = this.mapper.Map<IEnumerable<Page>, IEnumerable<MultiLevelPageDTO>>(pages);

            foreach (var page in levelPages)
            {
                page.ChildLevelPages = await GetChildLevelPagesAsync(page.Id);
            }

            return levelPages;
        }

        private IEnumerable<MultiLevelPageDTO> GetChildLevelPages(IEnumerable<MultiLevelPageDTO> pages, Guid parentId)
        {
            var childPages = pages.Where(t => t.ParentId == parentId);

            foreach (var page in childPages)
            {
                page.ChildLevelPages = GetChildLevelPages(pages, page.Id);
            }

            return childPages;
        }

        public async Task<IEnumerable<Page>> GetAllPagesAsync(Guid companyId)
        {
            var companies = new Guid[] { default, companyId };

            return await this.pageRepository.FindAllAsync(t => companies.Contains(t.CompanyId));
        }
    }
}
