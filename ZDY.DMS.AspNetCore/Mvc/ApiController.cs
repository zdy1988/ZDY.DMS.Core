using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZDY.DMS.AspNetCore.Auth;
using ZDY.DMS.AspNetCore.Bootstrapper.Module;
using ZDY.DMS.Querying.DataTableGateway;
using ZDY.DMS.Repositories;

namespace ZDY.DMS.AspNetCore.Mvc
{
    //[Authorize]
    [ApiRoute(ApiVersions.v1)]
    public abstract class ApiController<TServiceModule> : ControllerBase
        where TServiceModule : IServiceModule
    {
        private readonly IRepositoryContext repositoryContext;

        public ApiController()
        {
            this.repositoryContext = ServiceLocator.GetService<Func<Type, IRepositoryContext>>().Invoke(typeof(TServiceModule));
        }

        protected IRepositoryContext RepositoryContext => this.repositoryContext;

        protected IRepository<TKey, TEntity> GetRepository<TKey, TEntity>()
            where TKey : IEquatable<TKey>
            where TEntity : class, IEntity<TKey>
        {
            return this.RepositoryContext.GetRepository<TKey, TEntity>();
        }

        protected IDataTableGateway GetDataTableGateway()
        {
            return ServiceLocator.GetService<Func<Type, IDataTableGateway>>().Invoke(typeof(TServiceModule));
        }

        protected UserIdentity UserIdentity => this.HttpContext.GetUserIdentity();
    }
}
