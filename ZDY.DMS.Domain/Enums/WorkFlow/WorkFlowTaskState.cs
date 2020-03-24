using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZDY.DMS.Domain.Enums
{
    public enum WorkFlowTaskState
    {
        /// <summary>
        /// 等待中
        /// </summary>
        [Description("等待中")]
        [Category("1")]
        Waiting = -1,

        /// <summary>
        /// 待处理
        /// </summary>
        [Description("待处理")]
        [Category("1")]
        Pending = 0,

        /// <summary>
        /// 已打开
        /// </summary>
        [Description("已打开")]
        [Category("1")]
        Opened = 1,

        /// <summary>
        /// 已处理
        /// </summary>
        [Description("已处理")]
        [Category("1")]
        Handled = 2,

        /// <summary>
        /// 已退回
        /// </summary>
        [Description("已退回")]
        [Category("1")]
        Returned = 3,

        /// <summary>
        /// 被他人处理
        /// </summary>
        [Description("被他人处理")]
        [Category("1")]
        HandledByOthers = 4,

        /// <summary>
        /// 被他人退回
        /// </summary>
        [Description("被他人退回")]
        [Category("1")]
        ReturnedByOthers = 5
    }
}
