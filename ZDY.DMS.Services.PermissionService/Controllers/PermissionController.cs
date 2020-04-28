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
        public async Task SaveUserGroupMember(Guid groupId, Guid[] members)
        {
            if (groupId.Equals(default(Guid)))
            {
                throw new InvalidOperationException("用户组信息有误");
            }

            await userGroupMemberRepository.RemoveAsync(t => t.GroupId == groupId);

            foreach (var member in members.Where(t => !t.Equals(default)).Distinct())
            {
                await userGroupMemberRepository.AddAsync(new UserGroupMember
                {
                    GroupId = groupId,
                    UserId = member
                });
            }

            await this.RepositoryContext.CommitAsync();
        }

        [HttpPost]
        public async Task<IEnumerable<Guid>> FindUserGroupMember(Guid groupId)
        {
            if (groupId.Equals(default(Guid)))
            {
                throw new InvalidOperationException("用户组信息有误");
            }

            return (await userGroupMemberRepository.FindAllAsync(t => t.GroupId == groupId)).Select(t => t.UserId);
        }

        [HttpPost]
        public async Task SaveUserGroupPagePermission(Guid groupId, Guid[] permissions)
        {
            if (groupId.Equals(default(Guid)))
            {
                throw new InvalidOperationException("用户组信息有误");
            }

            await userGroupPagePermissionRepository.RemoveAsync(t => t.GroupId == groupId);

            if (permissions.Count() > 0)
            {
                foreach (var permission in permissions.Where(t => !t.Equals(default)).Distinct())
                {
                    await userGroupPagePermissionRepository.AddAsync(new UserGroupPagePermission
                    {
                        GroupId = groupId,
                        PageId = permission
                    });
                }
            }

            await this.RepositoryContext.CommitAsync();
        }

        [HttpPost]
        public async Task<IEnumerable<Guid>> FindUserGroupPagePermission(Guid groupId)
        {
            if (groupId.Equals(default(Guid)))
            {
                throw new InvalidOperationException("用户组信息有误");
            }

            return (await userGroupPagePermissionRepository.FindAllAsync(t => t.GroupId == groupId)).Select(t => t.PageId);
        }
    }
}
