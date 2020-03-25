using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDY.DMS.Models
{
    [Table("Work_Flow_Delegation")]
    public class WorkFlowDelegation : BaseEntity
    {
        /// <summary>
        /// 发布委托的人
        /// </summary>
        public Guid PublisherId { get; set; } = default;

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 委托流程ID,为空表示所有流程
        /// </summary>
        public Guid FlowId { get; set; } = default;

        /// <summary>
        /// 接收委托人
        /// </summary>
        public Guid DelegaterId { get; set; } = default;

        /// <summary>
        /// 设置时间
        /// </summary>
        public DateTime SetTime { get; set; }

        /// <summary>
        /// 备注说明
        /// </summary>
        public string Note { get; set; }
    }
}
