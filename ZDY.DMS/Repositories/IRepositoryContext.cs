using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZDY.DMS.Repositories
{
    /// <summary>
    /// 表示实现该接口的类型是仓储上下文。
    /// </summary>
    public interface IRepositoryContext : IDisposable
    {
        Guid ID { get; }

        object Session { get; }

        IRepository<TKey, TEntity> GetRepository<TKey, TEntity>()
            where TKey : IEquatable<TKey>
            where TEntity : class, IEntity<TKey>;

        void Commit();

        Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken));
    }

    public interface IRepositoryContext<out TSession> : IRepositoryContext
        where TSession : class
    {
        new TSession Session { get; }
    }
}
