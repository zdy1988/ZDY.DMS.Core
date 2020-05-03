using System;
using System.ComponentModel.DataAnnotations.Schema;
using ZDY.DMS.Services.Shared.Models;

namespace ZDY.DMS.Services.PermissionService.Models
{
    [Table("User_Group_Page_Permission")]
    public class UserGroupPagePermission : BaseEntity
    {
        public Guid GroupId { get; set; }
        public Guid PageId { get; set; }
    }
}
