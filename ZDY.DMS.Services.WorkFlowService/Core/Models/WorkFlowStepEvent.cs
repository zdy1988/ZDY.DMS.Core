using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.Services.WorkFlowService.Core.Models
{
    public partial class WorkFlowStep
    {
        /// <summary>
        /// 步骤提交前事件
        /// </summary>
        public string SubmitBeforeEvent { get; set; }

        /// <summary>
        /// 步骤提交后事件
        /// </summary>
        public string SubmitAfterEvent { get; set; }

        /// <summary>
        /// 步骤退回前事件
        /// </summary>
        public string BackBeforeEvent { get; set; }

        /// <summary>
        /// 步骤退回后事件
        /// </summary>
        public string BackAfterEvent { get; set; }

        /// <summary>
        /// 子流程激活前事件
        /// </summary>
        public string SubFlowActivationBeforeEvent { get; set; }

        /// <summary>
        /// 子流程激活后事件
        /// </summary>
        public string SubFlowActivationAfterEvent { get; set; }

        /// <summary>
        /// 子流程结束事件
        /// </summary>
        public string SubFlowFinishedEvent { get; set; }
    }
}
