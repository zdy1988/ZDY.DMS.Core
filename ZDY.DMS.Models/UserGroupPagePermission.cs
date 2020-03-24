using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDY.DMS.Models
{
    [Table("User_Group_Page_Permission")]
    public class UserGroupPagePermission : BaseEntity
    {
        public Guid GroupId { get; set; }
        public Guid PageId { get; set; }
    }
}
