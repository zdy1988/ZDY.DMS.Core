using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.DataPermission
{
    public interface IUserEntity<TKey> : IEntity<TKey>
         where TKey : IEquatable<TKey>
    {
        TKey UserId { get; set; }
    }
}
