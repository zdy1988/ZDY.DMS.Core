using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZDY.DMS.KeyGeneration
{
    public sealed class NullKeyGenerator<TKey> : IKeyGenerator<TKey, IEntity<TKey>>
        where TKey : IEquatable<TKey>
    {
        public TKey Generate(IEntity<TKey> entity)
        {
            return default(TKey);
        }
    }
}
