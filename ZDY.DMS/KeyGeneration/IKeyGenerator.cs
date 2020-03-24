using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZDY.DMS.KeyGeneration
{
    public interface IKeyGenerator<out TKey, in TEntity>
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
    {
        TKey Generate(TEntity entity);
    }
}
