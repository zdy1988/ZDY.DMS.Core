using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZDY.DMS.Repositories
{
    public abstract class RepositoryContext<TSession> : DisposableObject, IRepositoryContext<TSession>
        where TSession : class
    {
        public Guid ID { get; } = Guid.NewGuid();

        public TSession Session { get; }

        object IRepositoryContext.Session => this.Session;

        private readonly ConcurrentDictionary<Type, object> cachedRepositories = new ConcurrentDictionary<Type, object>();

        public RepositoryContext(TSession session)
        {
            this.Session = session;
        }

        public IRepository<TKey, TEntity> GetRepository<TKey, TEntity>()
            where TKey : IEquatable<TKey>
            where TEntity : class, IEntity<TKey> => (IRepository<TKey, TEntity>)cachedRepositories.GetOrAdd(typeof(TEntity), CreateRepository<TKey, TEntity>());

        protected abstract IRepository<TKey, TEntity> CreateRepository<TKey, TEntity>()
            where TKey : IEquatable<TKey>
            where TEntity : class, IEntity<TKey>;

        public abstract void Commit();

        public virtual Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Factory.StartNew(Commit, cancellationToken);
        }
    }
}
