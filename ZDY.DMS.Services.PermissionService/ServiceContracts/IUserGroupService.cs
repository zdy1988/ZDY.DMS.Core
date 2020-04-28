using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.Services.PermissionService.Models;

namespace ZDY.DMS.Services.PermissionService.ServiceContracts
{
    public interface IUserGroupService
    {
        Task<IEnumerable<UserGroup>> GetAllUserGroupAsync(Guid companyId);
    }
}
