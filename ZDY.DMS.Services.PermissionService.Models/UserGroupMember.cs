using System;
using System.ComponentModel.DataAnnotations.Schema;
using ZDY.DMS.Services.Shared.Models;

namespace ZDY.DMS.Services.PermissionService.Models
{
    [Table("User_Group_Member")]
    public class UserGroupMember : BaseEntity
    {
        public Guid GroupId { get; set; }
        public Guid UserId { get; set; }
    }
}
