using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZDY.DMS.Domain.Enums
{
    public enum WorkFlowTaskKinds
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        [Category("1")]
        Normal = 0,

        /// <summary>
        /// 指派
        /// </summary>
        [Description("指派")]
        [Category("1")]
        Assigne = 1,

        /// <summary>
        /// 委托
        /// </summary>
        [Description("委托")]
        [Category("1")]
        Delegate = 2,

        /// <summary>
        /// 转交
        /// </summary>
        [Description("转交")]
        [Category("1")]
        Redirect = 3,

        /// <summary>
        /// 退回
        /// </summary>
        [Description("退回")]
        [Category("1")]
        Return = 4,

        /// <summary>
        /// 抄送
        /// </summary>
        [Description("抄送")]
        [Category("1")]
        Copy = 5
    }
}
