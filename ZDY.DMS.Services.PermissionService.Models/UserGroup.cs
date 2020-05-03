using System;
using System.ComponentModel.DataAnnotations.Schema;
using ZDY.DMS.Services.Shared.Models;

namespace ZDY.DMS.Services.PermissionService.Models
{
    [Table("User_Group")]
    public class UserGroup : BaseEntity
    {
        public Guid CompanyId { get; set; }
        public string GroupName { get; set; }
    }
}
