using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.AspNetCore.Bootstrapper.Module;
using ZDY.DMS.Messaging;
using ZDY.DMS.Repositories;

namespace ZDY.DMS.AspNetCore.Messaging
{
    public abstract class MessageHandlerBase<TServiceModule, TMessage> : MessageHandler<TMessage>
        where TServiceModule : IServiceModule
        where TMessage : IMessage
    {
        private readonly IRepositoryContext repositoryContext;

        public MessageHandlerBase(Func<Type, IRepositoryContext> repositoryContextFactory)
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
