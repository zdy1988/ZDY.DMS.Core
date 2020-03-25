using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.Services.WorkFlowService.DataObjects
{
    public partial class WorkFlowStep
    {
        /// <summary>
        /// 步骤ID
        /// </summary>
        public Guid StepId { get; set; }
        /// <summary>
        /// 步骤类型 0 一般步骤 1 子流程步骤
        /// </summary>
        public int StepType { get; set; }
        /// <summary>
        /// 步骤名称
        /// </summary>
        public string StepName { get; set; }
        /// <summary>
        /// 意见显示 
        /// </summary>
        public bool IsShowComment { get; set; }
        /// <summary>
        /// 是否超期提示 
        /// </summary>
        public bool IsTimeoutReminding { get; set; }
        /// <summary>
        /// 审签类型 0无签批意见栏 1有签批意见(无须签章) 2有签批意见(须签章)
        /// </summary>
        public int SignatureType { get; set; }
        /// <summary>
        /// 时间限制
        /// </summary>
        public decimal TimeLimit { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 子流程ID
        /// </summary>
        public string SubFlowId { get; set; }
    }
}
