using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.DataPermission
{
    public interface ICompanyEntity<TKey> : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        TKey CompanyId { get; set; }
    }
}
