using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZDY.DMS.Services.WorkFlowService.Enums
{
    public enum WorkFlowStepKinds
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        [Category("1")]
        Normal = 0,

        /// <summary>
        /// 子流程
        /// </summary>
        [Description("子流程")]
        [Category("1")]
        SubFlow = 1
    }
}
