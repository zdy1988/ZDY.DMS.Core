using System;
using System.Collections.Generic;
using ZDY.DMS.Domain.Enums;
using ZDY.DMS.Models;

namespace ZDY.DMS.Services.WorkFlowService.DataObjects
{
    public class WorkFlowExecute
    {
        /// <summary>
        /// 流程安装数据
        /// </summary>
        public WorkFlowInstalled WorkFlowInstalled { get; set; }
        /// <summary>
        /// 流程ID
        /// </summary>
        public Guid FlowId { get; set; } = default;
        /// <summary>
        /// 流程ID
        /// </summary>
        public string FlowName { get; set; }
        /// <summary>
        /// 步骤ID
        /// </summary>
        public Guid StepId { get; set; } = default;
        /// <summary>
        /// 任务ID
        /// </summary>
        public Guid TaskId { get; set; } = default;
        /// <summary>
        /// 实例ID
        /// </summary>
        public Guid InstanceId { get; set; } = default;
        /// <summary>
        /// 分组ID
        /// </summary>
        public Guid GroupId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public WorkFlowExecuteKinds ExecuteType { get; set; }
        /// <summary>
        /// 发送人员
        /// </summary>
        public User Sender { get; set; }
        /// <summary>
        /// 接收的步骤和人员
        /// </summary>
        public Dictionary<Guid, List<User>> Steps { get; set; } = new Dictionary<Guid, List<User>>();
        /// <summary>
        /// 处理意见
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 是否签名
        /// </summary>
        public bool IsSign { get; set; }
        /// <summary>
        /// 签名密钥
        /// </summary>
        public string SignPassword { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public Guid CompanyId { get; set; }
    }
}
