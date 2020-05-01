using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.DataPermission
{
    public interface IUserGroupEntity<TKey> : ICompanyEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        TKey UserGroupId { get; set; }
    }
}
