using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.PermissionService.Models;
using ZDY.DMS.Services.PermissionService.ServiceContracts;

namespace ZDY.DMS.Services.PermissionService.Implementation
{
    public class PagePermissionService : IPagePermissionService
    {
        private readonly IRepositoryContext repositoryContext;

        public PagePermissionService(IRepositoryContext repositoryContext)
        {
            this.repositoryContext = repositoryContext;
        }

        public async Task<Guid[]> GetUserPagePermissionAsync(Guid userId)
        {
            var context = (DbContext)repositoryContext.Session;

            var pages = from pp in context.Set<UserGroupPagePermission>()
                        join ug in context.Set<UserGroup>()
                        on pp.GroupId equals ug.Id
                        join gm in context.Set<UserGroupMember>()
                        on new { groupId = ug.Id, memberId = userId } equals new { groupId = gm.GroupId, memberId = gm.UserId }
                        select pp.PageId;

            return await pages.ToArrayAsync();
        }
    }
}
