using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDY.DMS.Models
{
    [Table("User_Group")]
    public class UserGroup : BaseEntity
    {
        public Guid CompanyId { get; set; }
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
    }
}
