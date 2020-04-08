using System;
using ZDY.DMS.AspNetCore.Bootstrapper.Module;
using ZDY.DMS.KeyGeneration;
using ZDY.DMS.Querying.DataTableGateway;
using ZDY.DMS.Repositories;

namespace ZDY.DMS.AspNetCore.Bootstrapper.Service
{
    public abstract class ServiceBase<TServiceModule> : IServiceBase
        where TServiceModule : IServiceModule
    {
        private readonly IRepositoryContext repositoryContext;

        public ServiceBase()
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
    }

    public abstract class ServiceBase<TKey, TEntity, TServiceModule> : ServiceBase<TServiceModule>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
        where TServiceModule : IServiceModule
    {
        private readonly IRepository<TKey, TEntity> repository;
        private readonly IKeyGenerator<TKey, TEntity> keyGenerator;

        public ServiceBase(IKeyGenerator<TKey, TEntity> keyGenerator)
        {
            this.repository = this.GetRepository<TKey, TEntity>();
            this.keyGenerator = keyGenerator;
        }

        protected IRepository<TKey, TEntity> Repository => this.repository;

        protected IKeyGenerator<TKey, TEntity> KeyGenerator => this.keyGenerator;
    }

}
