using System;
using ZDY.DMS.Services.Common.Models;

namespace ZDY.DMS.Services.Common.Models
{
    public class Department : BaseEntity
    {
        public Guid CompanyId { get; set; }
        public Guid ParentId { get; set; }
        public string DepartmentName { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Note { get; set; }
        public int Order { get; set; } = 0;
    }
}
