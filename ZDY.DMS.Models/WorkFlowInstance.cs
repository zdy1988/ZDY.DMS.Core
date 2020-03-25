using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDY.DMS.Models
{
    [Table("Work_Flow_Instance")]
    public class WorkFlowInstance:BaseEntity 
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 流程ID
        /// </summary>
        public Guid FlowId { get; set; } = default;
        /// <summary>
        /// 流程名称
        /// </summary>
        public string FlowName { get; set; }
        /// <summary>
        /// 流程设计数据
        /// </summary>
        public string FlowDesignJson { get; set; }
        /// <summary>
        /// 流程数据
        /// </summary>
        public string FlowJson { get; set; }
        /// <summary>
        /// 表单数据
        /// </summary>
        public string FormJson { get; set; }
        /// <summary>
        /// 表单提交数据
        /// </summary>
        public string FormDataJson { get; set; }
        /// <summary>
        /// 最后执行的任务ID
        /// </summary>
        public Guid LastExecuteTaskId { get; set; } = default;
        /// <summary>
        /// 最后执行的任务ID
        /// </summary>
        public Guid LastExecuteStepId { get; set; } = default;
        /// <summary>
        /// 最后执行的任务名称
        /// </summary>
        public string LastExecuteStepName { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastModifyTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 创建人
        /// </summary>
        public Guid CreaterId { get; set; } = default;
        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreaterName { get; set; }
        /// <summary>
        /// 流程实例状态0审批中，1已完成，2流产
        /// </summary>
        public int State { get; set; } = 0;
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
