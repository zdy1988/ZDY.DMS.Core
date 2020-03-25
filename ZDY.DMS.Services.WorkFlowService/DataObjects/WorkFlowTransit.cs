using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.Services.WorkFlowService.DataObjects
{
    public class WrokFlowTransit
    {
        /// <summary>
        /// 条件ID
        /// </summary>
        public Guid TransitId { get; set; } = default;
        /// <summary>
        /// 条件名称
        /// </summary>
        public string TransitName { get; set; }
        /// <summary>
        /// 起始步骤ID
        /// </summary>
        public Guid FromStepId { get; set; } = default;
        /// <summary>
        /// 去往步骤ID
        /// </summary>
        public Guid ToStepId { get; set; } = default;
        /// <summary>
        /// 条件满足类型
        /// </summary>
        public int ConditionType { get; set; }
        /// <summary>
        /// 条件内容
        /// </summary>
        public string ConditionValue { get; set; }

    }
}
