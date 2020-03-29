using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.DataPermission;
using ZDY.DMS.KeyGeneration;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.OrganizationService.Models;

namespace ZDY.DMS.Services.OrganizationService
{
    //[Authorize(Roles = "Administrator")]
    public class CompanyController : ApiDataServiceController<Guid, Company>
    {
        public CompanyController(IRepositoryContext repositoryContext)
            :base(repositoryContext, new GuidKeyGenerator())
        { 
            
        }

        protected override void BeforeAdd(Company entity)
        {
            if (this.Repository.Exists(t => t.CompanyName == entity.CompanyName))
            {
                throw new InvalidOperationException("此公司名称已存在！");
            }
        }

        protected override void BeforeUpdate(Company original, Company entity)
        {
            if (this.Repository.Exists(t => t.CompanyName == entity.CompanyName && t.Id != entity.Id))
            {
                throw new InvalidOperationException("此公司名称已存在！");
            }

            base.BeforeUpdate(original, entity);
        }

        public override Task<Tuple<IEnumerable<Company>, int>> Search(SearchModel searchModel)
        {
            return base.Search(searchModel);
        }
    }
}
