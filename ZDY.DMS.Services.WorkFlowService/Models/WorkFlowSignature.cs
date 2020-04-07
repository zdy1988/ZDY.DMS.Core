using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZDY.DMS.Services.Common.Models;

namespace ZDY.DMS.Services.WorkFlowService.Models
{
    [Table("Work_Flow_Signature")]
    public class WorkFlowSignature : BaseEntity
    {
        public Guid UserId { get; set; } = default;

        public string Name { get; set; }

        [Required(ErrorMessage = "签名密匙是必须的，不能为空")]
        public string Password { get; set; }

        public string Note { get; set; }
    }
}
