using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.DataPermission;
using ZDY.DMS.KeyGeneration;
using ZDY.DMS.Models;
using ZDY.DMS.Repositories;

namespace ZDY.DMS.Services.AdminService
{
    //[Authorize(Roles = "Administrator")]
    public class LogController : ApiDataServiceController<Guid, Log>
    {
        public LogController(IRepositoryContext repositoryContext)
            : base(repositoryContext, new GuidKeyGenerator())
        {

        }

        public override Task<Tuple<IEnumerable<Log>, int>> Search(SearchModel searchModel)
        {
            return base.Search(searchModel);
        }

        public override Task<Log> Find(SearchModel searchModel)
        {
            throw new NotImplementedException();
        }

        public override Task<Log> FindByKey(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Task<Log> Add(Log entity)
        {
            throw new NotImplementedException();
        }

        public override Task<Log> Update(Log entity)
        {
            throw new NotImplementedException();
        }

        public override Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
