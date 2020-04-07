using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZDY.DMS.Services.Common.Models;

namespace ZDY.DMS.Services.WorkFlowService.Models
{
    [Table("Work_Flow_Signature")]
    public class WorkFlowSignature : BaseEntity
    {
        /// <summary>
        /// 签名所属用户Id
        /// </summary>
        public Guid UserId { get; set; } = default;

        /// <summary>
        /// 签名名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 签名密匙
        /// </summary>
        [Required(ErrorMessage = "签名密匙是必须的，不能为空")]
        public string Password { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastModifyTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 公司Id
        /// </summary>
        public Guid CompanyId { get; set; } = default;

        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }
    }
}
