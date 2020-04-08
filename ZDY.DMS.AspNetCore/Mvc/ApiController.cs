using System;
using Microsoft.AspNetCore.Mvc;
using ZDY.DMS.AspNetCore.Auth;
using ZDY.DMS.AspNetCore.Bootstrapper.Module;
using ZDY.DMS.Repositories;

namespace ZDY.DMS.AspNetCore.Mvc
{
    //[Authorize]
    //[ApiController]
    [ApiRoute(ApiVersions.v1)]
    public abstract class ApiController<TServiceModule> : ControllerBase
        where TServiceModule : IServiceModule
    {
        private readonly IRepositoryContext repositoryContext;

        public ApiController(Func<Type, IRepositoryContext> repositoryContextFactory)
        {
            this.repositoryContext = repositoryContextFactory.Invoke(typeof(TServiceModule));
        }

        protected IRepositoryContext RepositoryContext => this.repositoryContext;

        protected IRepository<TKey, TEntity> GetRepository<TKey, TEntity>()
            where TKey : IEquatable<TKey>
            where TEntity : class, IEntity<TKey>
        {
            return this.RepositoryContext.GetRepository<TKey, TEntity>();
        }

        protected UserIdentity UserIdentity => this.HttpContext.GetUserIdentity();
    }
}
