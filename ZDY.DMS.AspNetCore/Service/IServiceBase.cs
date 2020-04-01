using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.Repositories;

namespace ZDY.DMS.AspNetCore.Service
{
    public interface IServiceBase
    {
        IRepository<TKey, TEntity> GetRepository<TKey, TEntity>()
           where TKey : IEquatable<TKey>
           where TEntity : class, IEntity<TKey>;
    }
}
