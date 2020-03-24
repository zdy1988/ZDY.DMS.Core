using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDY.DMS.Models
{
    [Table("Work_Flow_Comment")]
    public class WorkFlowComment : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Comment { get; set; }
        public int Type { get; set; }
        public int Order { get; set; }
    }
}
