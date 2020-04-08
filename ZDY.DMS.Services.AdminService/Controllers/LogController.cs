using System;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.DataPermission;
using ZDY.DMS.KeyGeneration;
using ZDY.DMS.Services.AdminService.Models;

namespace ZDY.DMS.Services.AdminService.Controllers
{
    //[Authorize(Roles = "Administrator")]
    public class LogController : ApiDataServiceController<Guid, Log, AdminServiceModule>
    {
        public LogController()
            : base(new GuidKeyGenerator())
        {

        }

        public override Task<Log> Find(SearchModel searchModel)
        {
            throw new NotSupportedException();
        }

        public override Task<Log> FindByKey(Guid id)
        {
            throw new NotSupportedException();
        }

        public override Task<Log> Add(Log entity)
        {
            throw new NotSupportedException();
        }

        public override Task<Log> Update(Log entity)
        {
            throw new NotSupportedException();
        }

        public override Task Delete(Guid id)
        {
            throw new NotSupportedException();
        }
    }
}
