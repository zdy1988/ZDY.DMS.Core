using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.DataPermission;
using ZDY.DMS.KeyGeneration;
using ZDY.DMS.Models;
using ZDY.DMS.Repositories;

namespace ZDY.DMS.Services.PermissionService
{
    public class PermissionGroupController : ApiDataServiceController<Guid, UserGroup>
    {
        private readonly IRepository<Guid, UserGroupMember> userGroupMemberRepository;
        private readonly IRepository<Guid, UserGroupPagePermission> userGroupPagePermissionRepository;

        public PermissionGroupController(IRepositoryContext repositoryContext)
            : base(repositoryContext, new GuidKeyGenerator())
        {
            userGroupMemberRepository = repositoryContext.GetRepository<Guid, UserGroupMember>();
            userGroupPagePermissionRepository = repositoryContext.GetRepository<Guid, UserGroupPagePermission>();
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

        public override Task<Tuple<IEnumerable<UserGroup>, int>> Search(SearchModel searchModel)
        {
            return base.Search(searchModel);
        }
    }
}
