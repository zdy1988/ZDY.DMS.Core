using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.DataPermission
{
    public interface IDepartmentEntity<TKey> : ICompanyEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        TKey DepartmentId { get; set; }
    }
}
