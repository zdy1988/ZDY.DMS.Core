using System;
using ZDY.DMS.AspNetCore.Bootstrapper.Module;
using ZDY.DMS.Messaging;
using ZDY.DMS.Querying.DataTableGateway;
using ZDY.DMS.Repositories;

namespace ZDY.DMS.AspNetCore.Messaging
{
    public abstract class MessageHandlerBase<TServiceModule, TMessage> : MessageHandler<TMessage>
        where TServiceModule : IServiceModule
        where TMessage : IMessage
    {
        private readonly IRepositoryContext repositoryContext;

        public MessageHandlerBase()
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
}
