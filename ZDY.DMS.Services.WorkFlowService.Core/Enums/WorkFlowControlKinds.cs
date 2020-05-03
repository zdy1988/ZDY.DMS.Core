using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZDY.DMS.Services.WorkFlowService.Core.Enums
{
    public enum WorkFlowControlKinds
    {
        /// <summary>
        /// 由系统控制分支流转
        /// </summary>
        [Description("由系统控制分支流转")]
        [Category("1")]
        System = 0,

        /// <summary>
        /// 可单选一个分支流转
        /// </summary>
        [Description("可单选一个分支流转")]
        [Category("1")]
        SingleSelect = 1,

        /// <summary>
        /// 可多选几个分支流转
        /// </summary>
        [Description("可多选几个分支流转")]
        [Category("1")]
        MultiSelect = 2
    }
}
