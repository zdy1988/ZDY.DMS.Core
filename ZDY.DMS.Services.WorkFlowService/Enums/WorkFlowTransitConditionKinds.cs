using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZDY.DMS.Services.WorkFlowService.Enums
{
    public enum WorkFlowTransitConditionKinds
    {
        /// <summary>
        /// 无条件
        /// </summary>
        [Description("无条件")]
        [Category("1")]
        None = 0,

        /// <summary>
        /// SQL
        /// </summary>
        [Description("SQL")]
        [Category("1")]
        SQL = 1,

        /// <summary>
        /// 方法
        /// </summary>
        [Description("方法")]
        [Category("1")]
        Method = 2,

        /// <summary>
        /// 数据
        /// </summary>
        [Description("数据")]
        [Category("1")]
        Data = 3
    }
}
