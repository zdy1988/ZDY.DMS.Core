using System;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.DataPermission;
using ZDY.DMS.KeyGeneration;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.WorkFlowService.Models;

namespace ZDY.DMS.Services.WorkFlowService
{
    public class WorkFlowInstanceController : ApiDataServiceController<Guid, WorkFlowInstance>
    {
        public WorkFlowInstanceController(IRepositoryContext repositoryContext)
              : base(repositoryContext, new GuidKeyGenerator())
        {

        }

        public override Task<WorkFlowInstance> Add(WorkFlowInstance entity)
        {
            throw new NotImplementedException();
        }

        public override Task<WorkFlowInstance> Update(WorkFlowInstance entity)
        {
            throw new NotImplementedException();
        }

        public override Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Task<WorkFlowInstance> Find(SearchModel searchModel)
        {
            throw new NotImplementedException();
        }
    }
}
