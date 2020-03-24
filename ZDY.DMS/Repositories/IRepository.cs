using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZDY.DMS.Querying;
using ZDY.DMS.Querying.SearchModel.Model;

namespace ZDY.DMS.Repositories
{
    public interface IRepository<TKey, TEntity>
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
    {
        IRepositoryContext Context { get; }

        void Add(TEntity entity);

        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        void Update(TEntity entity);

        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        void UpdateByKey(TKey key, TEntity entity);

        Task UpdateByKeyAsync(TKey key, TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        void Remove(TEntity entity);

        Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        void RemoveByKey(TKey key);

        Task RemoveByKeyAsync(TKey key, CancellationToken cancellationToken = default(CancellationToken));

        bool Exists(Expression<Func<TEntity, bool>> conditions);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> conditions, CancellationToken cancellationToken = default(CancellationToken));

        int Count(Expression<Func<TEntity, bool>> conditions);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> conditions, CancellationToken cancellationToken = default(CancellationToken));

        TEntity Find(Expression<Func<TEntity, bool>> conditions);

        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> conditions, CancellationToken cancellationToken = default(CancellationToken));

        TEntity FindByKey(TKey key);

        Task<TEntity> FindByKeyAsync(TKey key, CancellationToken cancellationToken = default(CancellationToken));

        IEnumerable<TEntity> FindAll();

        Task<IEnumerable<TEntity>> FindAllAsync(CancellationToken cancellationToken = default(CancellationToken));

        IEnumerable<TEntity> FindAll(string orderField, bool isAsc);

        Task<IEnumerable<TEntity>> FindAllAsync(string orderField, bool isAsc, CancellationToken cancellationToken = default(CancellationToken));

        IEnumerable<TEntity> FindAll(Action<IOrderable<TEntity>> orderBy);

        Task<IEnumerable<TEntity>> FindAllAsync(Action<IOrderable<TEntity>> orderBy, CancellationToken cancellationToken = default(CancellationToken));

        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> conditions);

        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> conditions, CancellationToken cancellationToken = default(CancellationToken));

        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> conditions, string orderField, bool isAsc);

        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> conditions, string orderField, bool isAsc, CancellationToken cancellationToken = default(CancellationToken));

        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> conditions, Action<IOrderable<TEntity>> orderBy);

        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> conditions, Action<IOrderable<TEntity>> orderBy, CancellationToken cancellationToken = default(CancellationToken));

        Tuple<IEnumerable<TEntity>, int> FindAll(int pageIndex, int pageSize, string orderField, bool isAsc);

        Task<Tuple<IEnumerable<TEntity>, int>> FindAllAsync(int pageIndex, int pageSize, string orderField, bool isAsc, CancellationToken cancellationToken = default(CancellationToken));

        Tuple<IEnumerable<TEntity>, int> FindAll(int pageIndex, int pageSize, Action<IOrderable<TEntity>> orderBy);

        Task<Tuple<IEnumerable<TEntity>, int>> FindAllAsync(int pageIndex, int pageSize, Action<IOrderable<TEntity>> orderBy, CancellationToken cancellationToken = default(CancellationToken));

        Tuple<IEnumerable<TEntity>, int> FindAll(Expression<Func<TEntity, bool>> conditions, int pageIndex, int pageSize, string orderField, bool isAsc);

        Task<Tuple<IEnumerable<TEntity>, int>> FindAllAsync(Expression<Func<TEntity, bool>> conditions, int pageIndex, int pageSize, string orderField, bool isAsc, CancellationToken cancellationToken = default(CancellationToken));

        Tuple<IEnumerable<TEntity>, int> FindAll(Expression<Func<TEntity, bool>> conditions,int pageIndex, int pageSize, Action<IOrderable<TEntity>> orderBy);

        Task<Tuple<IEnumerable<TEntity>, int>> FindAllAsync(Expression<Func<TEntity, bool>> conditions, int pageIndex, int pageSize, Action<IOrderable<TEntity>> orderBy, CancellationToken cancellationToken = default(CancellationToken));

        TEntity Find(QueryModel queryModel);

        Task<TEntity> FindAsync(QueryModel queryModel, CancellationToken cancellationToken = default(CancellationToken));

        IEnumerable<TEntity> FindAll(QueryModel queryModel);

        Task<IEnumerable<TEntity>> FindAllAsync(QueryModel queryModel, CancellationToken cancellationToken = default(CancellationToken));

        IEnumerable<TEntity> FindAll(QueryModel queryModel, string orderField, bool isAsc);

        Task<IEnumerable<TEntity>> FindAllAsync(QueryModel queryModel, string orderField, bool isAsc, CancellationToken cancellationToken = default(CancellationToken));

        IEnumerable<TEntity> FindAll(QueryModel queryModel, Action<IOrderable<TEntity>> orderBy);

        Task<IEnumerable<TEntity>> FindAllAsync(QueryModel queryModel, Action<IOrderable<TEntity>> orderBy, CancellationToken cancellationToken = default(CancellationToken));

        Tuple<IEnumerable<TEntity>, int> FindAll(QueryModel queryModel, int pageIndex, int pageSize, string orderField, bool isAsc);

        Task<Tuple<IEnumerable<TEntity>, int>> FindAllAsync(QueryModel queryModel, int pageIndex, int pageSize, string orderField, bool isAsc, CancellationToken cancellationToken = default(CancellationToken));

        Tuple<IEnumerable<TEntity>, int> FindAll(QueryModel queryModel, int pageIndex, int pageSize, Action<IOrderable<TEntity>> orderBy);

        Task<Tuple<IEnumerable<TEntity>, int>> FindAllAsync(QueryModel queryModel, int pageIndex, int pageSize, Action<IOrderable<TEntity>> orderBy, CancellationToken cancellationToken = default(CancellationToken));

        int Update(Expression<Func<TEntity, bool>> conditions1, Expression<Func<TEntity, TEntity>> conditions2);

        Task<int> UpdateAsync(Expression<Func<TEntity, bool>> conditions1, Expression<Func<TEntity, TEntity>> conditions2);

        int Remove(Expression<Func<TEntity, bool>> conditions);

        Task<int> RemoveAsync(Expression<Func<TEntity, bool>> conditions);
    }
}
