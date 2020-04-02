using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.AspNetCore.Module;
using ZDY.DMS.Repositories;

namespace ZDY.DMS.AspNetCore.Service
{
    public abstract class ServiceBase<TServiceModule> : IServiceBase
        where TServiceModule : IServiceModule
    {
        private readonly IRepositoryContext repositoryContext;

        public ServiceBase(Func<Type, IRepositoryContext> repositoryContextFactory)
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
    }
}
