using System;
using System.Collections.Generic;
using ZDY.DMS.Services.WorkFlowService.Enums;

namespace ZDY.DMS.Services.WorkFlowService.DataObjects
{
    public class WorkFlowExecution
    {
        /// <summary>
        /// 流程安装数据
        /// </summary>
        public WorkFlowDefinition WorkFlowDefinition { get; set; }
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
        public Guid GroupId { get; set; } = default;
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public WorkFlowExecuteKinds ExecuteType { get; set; }
        /// <summary>
        /// 发送人员ID
        /// </summary>
        public WorkFlowUser Sender { get; set; }
        /// <summary>
        /// 接收的步骤和人员
        /// </summary>
        public Dictionary<Guid, List<WorkFlowUser>> Steps { get; set; } = new Dictionary<Guid, List<WorkFlowUser>>();
        /// <summary>
        /// 处理意见
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 是否签名
        /// </summary>
        public bool IsNeedSign { get; set; }
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
