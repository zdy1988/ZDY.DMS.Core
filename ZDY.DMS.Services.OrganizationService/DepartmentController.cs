using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.DataPermission;
using ZDY.DMS.KeyGeneration;
using ZDY.DMS.Models;
using ZDY.DMS.Repositories;

namespace ZDY.DMS.Services.OrganizationService
{
    public class DepartmentController : ApiDataServiceController<Guid, Department>
    {
        public DepartmentController(IRepositoryContext repositoryContext)
            : base(repositoryContext, new GuidKeyGenerator())
        {

        }

        protected override void BeforeAdd(Department entity)
        {
            if (this.Repository.Exists(t => t.CompanyId == entity.CompanyId && t.DepartmentName == entity.DepartmentName))
            {
                throw new InvalidOperationException("此部门名称已存在！");
            }
        }

        protected override void BeforeUpdate(Department original, Department entity)
        {
            if (this.Repository.Exists(t => t.CompanyId == entity.CompanyId && t.DepartmentName == entity.DepartmentName && t.Id != entity.Id))
            {
                throw new InvalidOperationException("此部门名称已存在！");
            }

            base.BeforeUpdate(original, entity);
        }

        public override Task<Tuple<IEnumerable<Department>, int>> Search(SearchModel searchModel)
        {
            return base.Search(searchModel);
        }
    }
}
