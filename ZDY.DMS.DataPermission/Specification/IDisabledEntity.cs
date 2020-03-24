using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.DataPermission
{
    public interface IDisabledEntity<TKey> : IEntity<TKey>
         where TKey : IEquatable<TKey>
    {
        bool IsDisabled { get; set; }
    }
}
