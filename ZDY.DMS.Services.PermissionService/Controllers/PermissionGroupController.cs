using System;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.KeyGeneration;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.PermissionService.Models;

namespace ZDY.DMS.Services.PermissionService.Controllers
{
    public class PermissionGroupController : ApiDataServiceController<Guid, UserGroup, PermissionServiceModule>
    {
        private readonly IRepository<Guid, UserGroupMember> userGroupMemberRepository;
        private readonly IRepository<Guid, UserGroupPagePermission> userGroupPagePermissionRepository;

        public PermissionGroupController()
            : base(new GuidKeyGenerator())
        {
            userGroupMemberRepository = this.RepositoryContext.GetRepository<Guid, UserGroupMember>();
            userGroupPagePermissionRepository = this.RepositoryContext.GetRepository<Guid, UserGroupPagePermission>();
        }

        protected override void BeforeAdd(UserGroup entity)
        {
            if (this.Repository.Exists(t => t.CompanyId == entity.CompanyId && (t.GroupName == entity.GroupName || t.GroupCode == entity.GroupCode)))
            {
                throw new InvalidOperationException("此权限组名称或代码已存在");
            }

            entity.CompanyId = this.UserIdentity.CompanyId;
        }

        protected override void BeforeUpdate(UserGroup original, UserGroup entity)
        {
            if (this.Repository.Exists(t => t.CompanyId == entity.CompanyId && (t.GroupName == entity.GroupName || t.GroupCode == entity.GroupCode) && t.Id != entity.Id))
            {
                throw new InvalidOperationException("此角色名称或代码已存在");
            }
        }

        protected override void BeforeDelete(Guid id)
        {
            userGroupPagePermissionRepository.Remove(t => t.GroupId == id);
            userGroupMemberRepository.Remove(t => t.GroupId == id);
        }
    }
}
