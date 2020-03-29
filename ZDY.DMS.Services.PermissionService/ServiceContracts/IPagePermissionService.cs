using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZDY.DMS.Services.PermissionService.ServiceContracts
{
    public interface IPagePermissionService
    {
        Task<Guid[]> GetUserPagePermissionAsync(Guid userId);
    }
}
