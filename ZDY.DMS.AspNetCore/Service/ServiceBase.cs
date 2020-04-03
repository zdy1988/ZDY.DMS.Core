using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.AspNetCore.Module;
using ZDY.DMS.KeyGeneration;
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

    public abstract class ServiceBase<TKey, TEntity, TServiceModule> : ServiceBase<TServiceModule>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
        where TServiceModule : IServiceModule
    {
        private readonly IRepository<TKey, TEntity> repository;
        private readonly IKeyGenerator<TKey, TEntity> keyGenerator;

        public ServiceBase(Func<Type, IRepositoryContext> repositoryContextFactory, IKeyGenerator<TKey, TEntity> keyGenerator)
            : base(repositoryContextFactory)
        {
            this.repository = this.GetRepository<TKey, TEntity>();
            this.keyGenerator = keyGenerator;
        }

        protected IRepository<TKey, TEntity> Repository => this.repository;

        protected IKeyGenerator<TKey, TEntity> KeyGenerator => this.keyGenerator;
    }

}
