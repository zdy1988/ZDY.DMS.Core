using System;
using Microsoft.AspNetCore.Mvc;
using ZDY.DMS.AspNetCore.Auth;
using ZDY.DMS.AspNetCore.Module;
using ZDY.DMS.Repositories;

namespace ZDY.DMS.AspNetCore.Mvc
{
    //[Authorize]
    //[ApiController]
    [ApiRoute(ApiVersions.v1)]
    public class ApiController<TServiceModule> : ControllerBase
        where TServiceModule : IServiceModule
    {
        private readonly IRepositoryContext repositoryContext;

        public ApiController(Func<Type, IRepositoryContext> repositoryContextFactory)
        {
            this.repositoryContext = repositoryContextFactory.Invoke(typeof(TServiceModule));
        }

        protected IRepositoryContext RepositoryContext => this.repositoryContext;

        protected UserIdentity UserIdentity => this.HttpContext.GetUserIdentity();
    }
}
