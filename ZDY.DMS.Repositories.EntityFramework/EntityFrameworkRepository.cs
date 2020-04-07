using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;
using ZDY.DMS.Querying;
using ZDY.DMS.Querying.SearchModel;
using ZDY.DMS.Querying.SearchModel.Model;

namespace ZDY.DMS.Repositories.EntityFramework
{
    internal sealed class EntityFrameworkRepository<TKey, TEntity> : Repository<TKey, TEntity>
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
    {
        private readonly DbContext dbContext;

        public EntityFrameworkRepository(IRepositoryContext context) : base(context)
        {
            this.dbContext = ((EntityFrameworkRepositoryContext)context).Session;
        }

        public override void Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentException("Can not add a null entity.");
            }

            this.dbContext.Set<TEntity>().Add(entity);
        }

        public override void RemoveByKey(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentException("Can not remove by a null key.");
            }

            var original = this.FindByKey(key);
            if (original != null)
            {
                this.dbContext.Set<TEntity>().Remove(original);
            }
        }

        public override void UpdateByKey(TKey key, TEntity entity)
        {
            if (key == null)
            {
                throw new ArgumentException("Can not update by a null key.");
            }

            if (entity == null)
            {
                throw new ArgumentException("Can not update a null entity.");
            }

            this.dbContext.Set<TEntity>().Update(entity);
        }

        public override TEntity Find(Expression<Func<TEntity, bool>> conditions)
        {
            return this.dbContext.Set<TEntity>().FirstOrDefault(conditions);
        }

        public override TEntity FindByKey(TKey key)
        {
            return this.dbContext.Set<TEntity>().FirstOrDefault(t => t.Id.Equals(key));
        }

        public override IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> conditions)
        {
            if (conditions == null)
            {
                conditions = _ => true;
            }

            var query = this.dbContext.Set<TEntity>().Where(conditions);

            return query;
        }

        public override IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> conditions, string orderField, bool isAsc)
        {
            if (conditions == null)
            {
                conditions = _ => true;
            }

            var query = this.dbContext.Set<TEntity>().Where(conditions);

            if (!string.IsNullOrEmpty(orderField))
            {
                query =  query.Sort(orderField, isAsc);
            }

            return query;
        }

        public override IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> conditions, Action<IOrderable<TEntity>> orderBy)
        {
            if (conditions == null)
            {
                conditions = _ => true;
            }

            var query = this.dbContext.Set<TEntity>().Where(conditions);

            if (orderBy != null)
            {
                query = query.Order(orderBy);
            }

            return query;
        }

        public override Tuple<IEnumerable<TEntity>, int> FindAll(Expression<Func<TEntity, bool>> conditions, int pageIndex, int pageSize, string orderField, bool isAsc)
        {
            if (pageIndex <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex), "The page index should be greater than 0.");
            }

            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "The page size should be greater than 0.");
            }

            if (conditions == null)
            {
                conditions = _ => true;
            }

            var query = this.dbContext.Set<TEntity>().Where(conditions);

            var total = query.Count();

            var list = query.Sort<TEntity>(orderField, isAsc).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return new Tuple<IEnumerable<TEntity>, int>(list, total);
        }

        public override Tuple<IEnumerable<TEntity>, int> FindAll(Expression<Func<TEntity, bool>> conditions, int pageIndex, int pageSize, Action<IOrderable<TEntity>> orderBy)
        {
            if (pageIndex <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex), "The page index should be greater than 0.");
            }

            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "The page size should be greater than 0.");
            }

            if (conditions == null)
            {
                conditions = _ => true;
            }

            var query = this.dbContext.Set<TEntity>().Where(conditions);

            var total = query.Count();

            var list = query.Order<TEntity>(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return new Tuple<IEnumerable<TEntity>, int>(list, total);
        }

        public override TEntity Find(QueryModel queryModel)
        {
            var query = this.dbContext.Set<TEntity>().Where(queryModel);

            return query.FirstOrDefault();
        }

        public override IEnumerable<TEntity> FindAll(QueryModel queryModel)
        {
            var query = this.dbContext.Set<TEntity>().Where(queryModel);

            return query;
        }

        public override IEnumerable<TEntity> FindAll(QueryModel queryModel, string orderField, bool isAsc)
        {
            var query = this.dbContext.Set<TEntity>().Where(queryModel);

            if (!string.IsNullOrEmpty(orderField))
            {
                query = query.Sort(orderField, isAsc);
            }

            return query;
        }

        public override IEnumerable<TEntity> FindAll(QueryModel queryModel, Action<IOrderable<TEntity>> orderBy)
        {
            var query = this.dbContext.Set<TEntity>().Where(queryModel);

            if (orderBy != null)
            {
                query = query.Order(orderBy);
            }

            return query;
        }

        public override Tuple<IEnumerable<TEntity>, int> FindAll(QueryModel queryModel, int pageIndex, int pageSize, string orderField, bool isAsc)
        {
            if (pageIndex <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex), "The page index should be greater than 0.");
            }

            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "The page size should be greater than 0.");
            }

            var query = this.dbContext.Set<TEntity>().Where(queryModel);

            var total = query.Count();

            var list = query.Sort<TEntity>(orderField, isAsc).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return new Tuple<IEnumerable<TEntity>, int>(list, total);
        }

        public override Tuple<IEnumerable<TEntity>, int> FindAll(QueryModel queryModel, int pageIndex, int pageSize, Action<IOrderable<TEntity>> orderBy)
        {
            if (pageIndex <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex), "The page index should be greater than 0.");
            }

            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "The page size should be greater than 0.");
            }

            var query = this.dbContext.Set<TEntity>().Where(queryModel);

            var total = query.Count();

            var list = query.Order<TEntity>(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return new Tuple<IEnumerable<TEntity>, int>(list, total);
        }

        public override int Update(Expression<Func<TEntity, bool>> conditions1, Expression<Func<TEntity, TEntity>> conditions2)
        {
            return this.dbContext.Set<TEntity>().Where<TEntity>(conditions1).Update(conditions2);
        }

        public override Task<int> UpdateAsync(Expression<Func<TEntity, bool>> conditions1, Expression<Func<TEntity, TEntity>> conditions2)
        {
            //string commandText;
            //return this.dbContext.Set<TEntity>().Where<TEntity>(conditions1).UpdateAsync(conditions2,
            //     x => {
            //         x.Executing = command => {
            //             commandText = command.CommandText;
            //         };
            //     });

            return this.dbContext.Set<TEntity>().Where<TEntity>(conditions1).UpdateAsync(conditions2);
        }

        public override int Remove(Expression<Func<TEntity, bool>> conditions)
        {
            return this.dbContext.Set<TEntity>().Where<TEntity>(conditions).Delete();
        }

        public override Task<int> RemoveAsync(Expression<Func<TEntity, bool>> conditions)
        {
            return this.dbContext.Set<TEntity>().Where<TEntity>(conditions).DeleteAsync();
        }
    }
}
