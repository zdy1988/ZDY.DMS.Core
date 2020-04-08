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
        public string BeforeSubmitInterceptor { get; set; }

        /// <summary>
        /// 步骤提交后事件
        /// </summary>
        public string AfterSubmitInterceptor { get; set; }

        /// <summary>
        /// 步骤退回前事件
        /// </summary>
        public string BeforeBackInterceptor { get; set; }

        /// <summary>
        /// 步骤退回后事件
        /// </summary>
        public string AfterBackInterceptor { get; set; }

        /// <summary>
        /// 子流程激活前事件
        /// </summary>
        public string BeforeSubFlowActivationInterceptor { get; set; }

        /// <summary>
        /// 子流程激活后事件
        /// </summary>
        public string AfterSubFlowActivationInterceptor { get; set; }

        /// <summary>
        /// 子流程结束事件
        /// </summary>
        public string SubFlowFinishedInterceptor { get; set; }
    }
}
