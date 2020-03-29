using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZDY.DMS.Services.WorkFlowService.Enums
{
    public enum WorkFlowBackTacticKinds
    {
        /// <summary>
        /// 不能退回
        /// </summary>
        [Description("不能退回")]
        [Category("1")]
        UnableToReturn = 0,

        /// <summary>
        /// 可以退回
        /// </summary>
        [Description("可以退回")]
        [Category("1")]
        AllowReturn = 1,

        /// <summary>
        /// 依据策略退回
        /// </summary>
        [Description("依据策略退回")]
        [Category("1")]
        ReturnByTactic = 2
    }
}
