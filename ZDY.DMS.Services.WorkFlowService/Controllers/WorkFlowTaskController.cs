using System;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.DataPermission;
using ZDY.DMS.KeyGeneration;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.WorkFlowService.Models;

namespace ZDY.DMS.Services.WorkFlowService
{
    public class WorkFlowTaskController : ApiDataServiceController<Guid, WorkFlowTask, WorkFlowServiceModule>
    {
        public WorkFlowTaskController(Func<Type, IRepositoryContext> repositoryContextFactory)
            : base(repositoryContextFactory, new GuidKeyGenerator())
        {

        }

        public override Task<WorkFlowTask> Add(WorkFlowTask entity)
        {
            throw new NotSupportedException();
        }

        public override Task<WorkFlowTask> Update(WorkFlowTask entity)
        {
            throw new NotSupportedException();
        }

        public override Task Delete(Guid id)
        {
            throw new NotSupportedException();
        }

        public override Task<WorkFlowTask> Find(SearchModel searchModel)
        {
            throw new NotSupportedException();
        }
    }
}
