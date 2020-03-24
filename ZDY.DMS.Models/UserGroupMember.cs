using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDY.DMS.Models
{
    [Table("User_Group_Member")]
    public class UserGroupMember : BaseEntity
    {
        public Guid GroupId { get; set; }
        public Guid UserId { get; set; }
    }
}
