using System;
using System.ComponentModel.DataAnnotations.Schema;
using ZDY.DMS.Services.Shared.Models;

namespace ZDY.DMS.Services.AdminService.Models
{
    [Table("Page_Action")]
    public class PageAction : BaseEntity
    {
        public Guid PageId { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
    }
}
