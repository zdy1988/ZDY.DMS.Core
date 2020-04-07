using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.Services.WorkFlowService.Enums;

namespace ZDY.DMS.Services.WorkFlowService.Core.Models
{
    public class WorkFlowExecute
    {
        public Guid TaskId { get; set; } = default;

        /// <summary>
        /// 处理类型
        /// </summary>
        public WorkFlowExecuteKinds ExecuteType { get; set; }

        /// <summary>
        /// 处理意见
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 签名密钥
        /// </summary>
        public string SignaturePassword { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 发送人员
        /// </summary>
        public WorkFlowUser Sender { get; set; }

        /// <summary>
        /// 自定义到达的步骤Id和人员
        /// </summary>
        public Dictionary<Guid, List<WorkFlowUser>> Steps { get; set; }
    }
}
