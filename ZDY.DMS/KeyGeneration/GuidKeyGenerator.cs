using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZDY.DMS.KeyGeneration
{
    public sealed class GuidKeyGenerator : IKeyGenerator<Guid, IEntity<Guid>>
    {
        public Guid Generate(IEntity<Guid> entity)
        {
            return Guid.NewGuid();
        }
    }
}
