using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZDY.DMS.Services.WorkFlowService.Enums
{
    public enum WorkFlowSubFlowTacticKinds
    {
        /// <summary>
        /// 子流程通过时才能处理
        /// </summary>
        [Description("子流程通过时才能处理")]
        [Category("1")]
        SubFlowCompleted = 0,

        /// <summary>
        /// 子流程结束时即可处理
        /// </summary>
        [Description("子流程结束时即可处理")]
        [Category("1")]
        SubFlowFinished = 1,

        /// <summary>
        /// 子流程开启时即可处理
        /// </summary>
        [Description("子流程开启时即可处理")]
        [Category("1")]
        SubFlowStarted = 2
    }
}
