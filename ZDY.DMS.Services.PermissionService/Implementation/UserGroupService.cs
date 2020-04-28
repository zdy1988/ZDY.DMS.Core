using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore.Bootstrapper.Service;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.PermissionService.Models;
using ZDY.DMS.Services.PermissionService.ServiceContracts;

namespace ZDY.DMS.Services.PermissionService.Implementation
{
    public class UserGroupService : ServiceBase<PermissionServiceModule>, IUserGroupService
    {
        private readonly IRepository<Guid, UserGroup> userGroupService;

        public UserGroupService()
        {
            this.userGroupService = this.GetRepository<Guid, UserGroup>();
        }

        public async Task<IEnumerable<UserGroup>> GetAllUserGroupAsync(Guid companyId)
        {
            return await this.userGroupService.FindAllAsync(t => t.CompanyId == companyId);
        }
    }
}
