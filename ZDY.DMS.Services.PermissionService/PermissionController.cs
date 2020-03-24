using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.Models;
using ZDY.DMS.Repositories;

namespace ZDY.DMS.Services.PermissionService
{
    public class PermissionController : ApiController
    {
        private readonly IRepositoryContext repositoryContext;
        private readonly IRepository<Guid, UserGroupMember> userGroupMemberRepository;
        private readonly IRepository<Guid, UserGroupPagePermission> userGroupPagePermissionRepository;
        private readonly IRepository<Guid, UserGroupActionPermission> userGroupActionPermissionRepository;

        public PermissionController(IRepositoryContext repositoryContext)
        {
            this.repositoryContext = repositoryContext;
            this.userGroupMemberRepository = repositoryContext.GetRepository<Guid, UserGroupMember>();
            this.userGroupPagePermissionRepository = repositoryContext.GetRepository<Guid, UserGroupPagePermission>();
            this.userGroupActionPermissionRepository = repositoryContext.GetRepository<Guid, UserGroupActionPermission>();
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

            await this.repositoryContext.CommitAsync();
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

            await this.repositoryContext.CommitAsync();
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
