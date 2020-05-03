using System;
using System.ComponentModel.DataAnnotations.Schema;
using ZDY.DMS.Services.Shared.Models;

namespace ZDY.DMS.Services.PermissionService.Models
{
    [Table("User_Group_Action_Permission")]
    public class UserGroupActionPermission : BaseEntity
    {
        public Guid GroupId { get; set; }
        public Guid PageId { get; set; }
        public Guid PageActionId { get; set; }
    }
}
