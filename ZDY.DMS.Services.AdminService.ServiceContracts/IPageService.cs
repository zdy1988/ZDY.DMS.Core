using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZDY.DMS.Services.AdminService.DataTransferObjects;
using ZDY.DMS.Services.AdminService.Models;

namespace ZDY.DMS.Services.AdminService.ServiceContracts
{
    public interface IPageService
    {
        Task<IEnumerable<MultiLevelPageDTO>> GetMultiLevelPagesAsync(Guid[] pageIdRanges, Guid companyId);

        Task<IEnumerable<MultiLevelPageDTO>> GetMultiLevelPagesAsync();

        Task<IEnumerable<MultiLevelPageDTO>> GetChildLevelPagesAsync(Guid parentId);

        Task<IEnumerable<Page>> GetAllPagesAsync(Guid companyId);
    }
}
