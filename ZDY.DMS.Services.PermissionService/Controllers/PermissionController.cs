using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.PermissionService.Models;

namespace ZDY.DMS.Services.PermissionService.Controllers
{
    public class PermissionController : ApiController<PermissionServiceModule>
    {
        private readonly IRepository<Guid, UserGroupMember> userGroupMemberRepository;
        private readonly IRepository<Guid, UserGroupPagePermission> userGroupPagePermissionRepository;
        private readonly IRepository<Guid, UserGroupActionPermission> userGroupActionPermissionRepository;

        public PermissionController()
        {
            this.userGroupMemberRepository = this.RepositoryContext.GetRepository<Guid, UserGroupMember>();
            this.userGroupPagePermissionRepository = this.RepositoryContext.GetRepository<Guid, UserGroupPagePermission>();
            this.userGroupActionPermissionRepository = this.RepositoryContext.GetRepository<Guid, UserGroupActionPermission>();
        }

        [HttpPost]
        public async Task AddUserGroupMember(Guid groupId, List<UserGroupMember> members)
        {
            if (groupId.Equals(default(Guid)))
            {
                throw new InvalidOperationException("用户组信息有误");
            }

            await userGroupMemberRepository.RemoveAsync(t => t.GroupId == groupId);

            foreach (var member in members.Distinct())
            {
                member.GroupId = groupId;
                await userGroupMemberRepository.AddAsync(member);
            }

            await this.RepositoryContext.CommitAsync();
        }

        [HttpPost]
        public async Task<IEnumerable<UserGroupMember>> FindUserGroupMember(Guid groupId)
        {
            if (groupId.Equals(default(Guid)))
            {
                throw new InvalidOperationException("用户组信息有误");
            }

            return await userGroupMemberRepository.FindAllAsync(t => t.GroupId == groupId);
        }

        [HttpPost]
        public async Task SetUserGroupPagePermission(Guid groupId, List<UserGroupPagePermission> permissions)
        {
            if (groupId.Equals(default(Guid)))
            {
                throw new InvalidOperationException("用户组信息有误");
            }

            await userGroupPagePermissionRepository.RemoveAsync(t => t.GroupId == groupId);

            if (permissions.Count > 0)
            {
                var distinctResult = permissions.Where(t => !t.PageId.Equals(default))
                                                .GroupBy(t => t.PageId)
                                                .Select(t => t.First());

                foreach (var permission in distinctResult)
                {
                    permission.GroupId = groupId;
                    await userGroupPagePermissionRepository.AddAsync(permission);
                }
            }

            await this.RepositoryContext.CommitAsync();
        }

        [HttpPost]
        public async Task<IEnumerable<UserGroupPagePermission>> FindUserGroupPagePermission(Guid groupId)
        {
            if (groupId.Equals(default(Guid)))
            {
                throw new InvalidOperationException("用户组信息有误");
            }

            return await userGroupPagePermissionRepository.FindAllAsync(t => t.GroupId == groupId);
        }
    }
}
