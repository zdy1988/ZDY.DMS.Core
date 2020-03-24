using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZDY.DMS.Querying;
using ZDY.DMS.Querying.SearchModel.Model;

namespace ZDY.DMS.Repositories
{
    public abstract class Repository<TKey, TEntity> : IRepository<TKey, TEntity>
       where TKey : IEquatable<TKey>
       where TEntity : class, IEntity<TKey>
    {
        protected Repository(IRepositoryContext context)
        {
            this.Context = context;
        }

        public IRepositoryContext Context { get; }

        public abstract void Add(TEntity entity);

        public virtual Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() => this.Add(entity), cancellationToken);
        }

        public virtual void Remove(TEntity entity)
        {
            this.RemoveByKey(entity.Id);
        }

        public virtual Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            return this.RemoveByKeyAsync(entity.Id, cancellationToken);
        }

        public abstract void RemoveByKey(TKey key);

        public virtual Task RemoveByKeyAsync(TKey key, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() => this.RemoveByKey(key), cancellationToken);
        }

        public virtual void Update(TEntity entity)
        {
            this.UpdateByKey(entity.Id, entity);
        }

        public virtual Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            return this.UpdateByKeyAsync(entity.Id, entity, cancellationToken);
        }

        public abstract void UpdateByKey(TKey key, TEntity entity);

        public virtual Task UpdateByKeyAsync(TKey key, TEntity entity, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() => this.UpdateByKey(key, entity), cancellationToken);
        }

        public virtual bool Exists(Expression<Func<TEntity, bool>> conditions)
        {
            return this.FindAll(conditions).Count() > 0;
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> conditions, CancellationToken cancellationToken = default)
        {
            return (await this.FindAllAsync(conditions, cancellationToken)).Count() > 0;
        }

        public virtual int Count(Expression<Func<TEntity, bool>> conditions)
        {
            return this.FindAll(conditions).Count();
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> conditions, CancellationToken cancellationToken = default)
        {
            return (await this.FindAllAsync(conditions, cancellationToken)).Count();
        }

        public abstract TEntity Find(Expression<Func<TEntity, bool>> conditions);

        public virtual Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> conditions, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() => this.Find(conditions), cancellationToken);
        }

        public abstract TEntity FindByKey(TKey key);

        public virtual Task<TEntity> FindByKeyAsync(TKey key, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() => this.FindByKey(key), cancellationToken);
        }

        public virtual IEnumerable<TEntity> FindAll()
        {
            return this.FindAll(_ => true, null);
        }

        public virtual Task<IEnumerable<TEntity>> FindAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(this.FindAll());
        }

        public virtual IEnumerable<TEntity> FindAll(string orderField, bool isAsc)
        {
            return this.FindAll(_ => true, orderField, isAsc);
        }

        public virtual Task<IEnumerable<TEntity>> FindAllAsync(string orderField, bool isAsc, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(this.FindAll(orderField, isAsc));
        }

        public virtual IEnumerable<TEntity> FindAll(Action<IOrderable<TEntity>> orderBy)
        {
            return this.FindAll(_ => true, orderBy);
        }

        public virtual Task<IEnumerable<TEntity>> FindAllAsync(Action<IOrderable<TEntity>> orderBy, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(this.FindAll(orderBy));
        }

        public abstract IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> conditions);

        public virtual Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> conditions, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(this.FindAll(conditions));
        }

        public abstract IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> conditions, string orderField, bool isAsc);

        public virtual Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> conditions, string orderField, bool isAsc, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(this.FindAll(conditions, orderField, isAsc));
        }

        public abstract IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> conditions, Action<IOrderable<TEntity>> orderBy);

        public virtual Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> conditions, Action<IOrderable<TEntity>> orderBy, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(this.FindAll(conditions, orderBy));
        }

        public virtual Tuple<IEnumerable<TEntity>, int> FindAll(int pageIndex, int pageSize, string orderField, bool isAsc)
        {
            return this.FindAll(_ => true, pageIndex, pageSize, orderField, isAsc);
        }

        public virtual Task<Tuple<IEnumerable<TEntity>, int>> FindAllAsync(int pageIndex, int pageSize, string orderField, bool isAsc, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(this.FindAll(pageIndex, pageSize, orderField, isAsc));
        }

        public virtual Tuple<IEnumerable<TEntity>, int> FindAll(int pageIndex, int pageSize, Action<IOrderable<TEntity>> orderBy)
        {
            return this.FindAll(_ => true, pageIndex, pageSize, orderBy);
        }

        public virtual Task<Tuple<IEnumerable<TEntity>, int>> FindAllAsync(int pageIndex, int pageSize, Action<IOrderable<TEntity>> orderBy, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(this.FindAll(pageIndex, pageSize, orderBy));
        }

        public abstract Tuple<IEnumerable<TEntity>, int> FindAll(Expression<Func<TEntity, bool>> conditions, int pageIndex, int pageSize, string orderField, bool isAsc);

        public virtual Task<Tuple<IEnumerable<TEntity>, int>> FindAllAsync(Expression<Func<TEntity, bool>> conditions, int pageIndex, int pageSize, string orderField, bool isAsc, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(this.FindAll(conditions, pageIndex, pageSize, orderField, isAsc));
        }

        public abstract Tuple<IEnumerable<TEntity>, int> FindAll(Expression<Func<TEntity, bool>> conditions, int pageIndex, int pageSize, Action<IOrderable<TEntity>> orderBy);

        public virtual Task<Tuple<IEnumerable<TEntity>, int>> FindAllAsync(Expression<Func<TEntity, bool>> conditions, int pageIndex, int pageSize, Action<IOrderable<TEntity>> orderBy, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(this.FindAll(conditions, pageIndex, pageSize, orderBy));
        }

        public abstract TEntity Find(QueryModel queryModel);

        public virtual Task<TEntity> FindAsync(QueryModel queryModel, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Factory.StartNew(() => this.Find(queryModel), cancellationToken);
        }

        public abstract IEnumerable<TEntity> FindAll(QueryModel queryModel);

        public virtual Task<IEnumerable<TEntity>> FindAllAsync(QueryModel queryModel, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(this.FindAll(queryModel));
        }

        public abstract IEnumerable<TEntity> FindAll(QueryModel queryModel, string orderField, bool isAsc);

        public virtual Task<IEnumerable<TEntity>> FindAllAsync(QueryModel queryModel, string orderField, bool isAsc, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(this.FindAll(queryModel, orderField, isAsc));
        }

        public abstract IEnumerable<TEntity> FindAll(QueryModel queryModel, Action<IOrderable<TEntity>> orderBy);

        public virtual Task<IEnumerable<TEntity>> FindAllAsync(QueryModel queryModel, Action<IOrderable<TEntity>> orderBy, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(this.FindAll(queryModel, orderBy));
        }

        public abstract Tuple<IEnumerable<TEntity>, int> FindAll(QueryModel queryModel, int pageIndex, int pageSize, string orderField, bool isAsc);

        public virtual Task<Tuple<IEnumerable<TEntity>, int>> FindAllAsync(QueryModel queryModel, int pageIndex, int pageSize, string orderField, bool isAsc, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(this.FindAll(queryModel, pageIndex, pageSize, orderField, isAsc));
        }

        public abstract Tuple<IEnumerable<TEntity>, int> FindAll(QueryModel queryModel, int pageIndex, int pageSize, Action<IOrderable<TEntity>> orderBy);

        public Task<Tuple<IEnumerable<TEntity>, int>> FindAllAsync(QueryModel queryModel, int pageIndex, int pageSize, Action<IOrderable<TEntity>> orderBy, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(this.FindAll(queryModel, pageIndex, pageSize, orderBy));
        }

        public virtual int Update(Expression<Func<TEntity, bool>> conditions1, Expression<Func<TEntity, TEntity>> conditions2)
        {
            throw new NotImplementedException();
        }

        public virtual Task<int> UpdateAsync(Expression<Func<TEntity, bool>> conditions1, Expression<Func<TEntity, TEntity>> conditions2)
        {
            throw new NotImplementedException();
        }

        public virtual int Remove(Expression<Func<TEntity, bool>> conditions)
        {
            throw new NotImplementedException();
        }

        public virtual Task<int> RemoveAsync(Expression<Func<TEntity, bool>> conditions)
        {
            throw new NotImplementedException();
        }
    }
}
