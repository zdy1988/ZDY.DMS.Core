using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZDY.DMS.Services.Common.DataTransferObjects;

namespace ZDY.DMS.Services.AdminService.ServiceContracts
{
    public interface IPageService
    {
        Task<IEnumerable<MultiLevelPageDTO>> GetMultiLevelPagesAsync(Guid[] pageIdRanges, Guid companyId);

        Task<IEnumerable<MultiLevelPageDTO>> GetMultiLevelPagesAsync();

        Task<IEnumerable<MultiLevelPageDTO>> GetChildLevelPagesAsync(Guid parentId);
    }
}
