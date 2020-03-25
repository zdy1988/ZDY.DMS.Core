using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDY.DMS.Models
{
    [Table("Work_Flow_Task")]
    public class WorkFlowTask : BaseEntity
    {
        /// <summary>
        /// 上一任务ID
        /// </summary>
        public Guid PrevTaskId { get; set; } = default;

        /// <summary>
        /// 上一步骤ID
        /// </summary>
        public Guid PrevStepId { get; set; } = default;

        /// <summary>
        /// 流程ID
        /// </summary>
        public Guid FlowId { get; set; } = default;

        /// <summary>
        /// 流程名称
        /// </summary>
        public string FlowName { get; set; }

        /// <summary>
        /// 当前步骤ID
        /// </summary>
        public Guid StepId { get; set; } = default;

        /// <summary>
        /// 当前步骤名称
        /// </summary>
        public string StepName { get; set; }

        /// <summary>
        /// 当前实例ID
        /// </summary>
        public Guid InstanceId { get; set; } = default;

        /// <summary>
        /// GroupID
        /// </summary>
        public Guid GroupId { get; set; } = default;

        /// <summary>
        /// 任务类型 0正常 1指派 2委托 3转交 4退回 5抄送
        /// </summary>
        public int Type { get; set; } = 0;

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 发送人
        /// </summary>
        public Guid SenderId { get; set; } = default;

        /// <summary>
        /// 发送人姓名
        /// </summary>
        public string SenderName { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 接收人员ID
        /// </summary>
        public Guid ReceiverId { get; set; } = default;

        /// <summary>
        /// 接收人员姓名
        /// </summary>
        public string ReceiverName { get; set; }

        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime ReceiveTime { get; set; }

        /// <summary>
        /// 打开时间
        /// </summary>
        public Nullable<DateTime> OpenedTime { get; set; }

        /// <summary>
        /// 计划处理时间
        /// </summary>
        public Nullable<DateTime> PlannedTime { get; set; }

        /// <summary>
        /// 处理完成时间
        /// </summary>
        public Nullable<DateTime> ExecutedTime { get; set; }

        /// <summary>
        /// 意见
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 是否需要签名
        /// </summary>
        public bool IsNeedSign { get; set; }

        /// <summary>
        /// 状态 -1 等待中的任务 0 待处理 1打开 2完成 3退回 4他人已处理 5他人已退回
        /// </summary>
        public int State { get; set; } = 0;

        /// <summary>
        /// 其它说明
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 子流程实例ID
        /// </summary>
        public Guid SubFlowInstanceId { get; set; } = default;

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool IsDisabled { get; set; } = false;

        /// <summary>
        /// 公司ID
        /// </summary>
        public Guid CompanyId { get; set; } = default;
    }
}
