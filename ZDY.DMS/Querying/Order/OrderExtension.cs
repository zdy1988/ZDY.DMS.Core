using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ZDY.DMS.Querying
{
    public static class OrderExtension
    {
        public static IQueryable<TEntity> Order<TEntity>(this IQueryable<TEntity> table, Action<IOrderable<TEntity>> orderBy) where TEntity : class
        {
            if (orderBy == null)
                throw new InvalidOperationException("基于委托的排序必须指定排序字段和排序顺序");

            Contract.Requires(table != null);
            var orderable = new Orderable<TEntity>(table);
            orderBy(orderable);
            return orderable.Queryable;
        }
    }
}
