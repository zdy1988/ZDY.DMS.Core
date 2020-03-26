using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZDY.DMS.Domain.Enums
{
    public enum WorkFlowInstanceState
    {
        /// <summary>
        /// 审批中
        /// </summary>
        [Description("审批中")]
        [Category("1")]
        Approving = 0,

        /// <summary>
        /// 已通过
        /// </summary>
        [Description("已通过")]
        [Category("1")]
        Completed = 1,

        /// <summary>
        /// 不通过
        /// </summary>
        [Description("不通过")]
        [Category("1")]
        Rejected = 2,

        /// <summary>
        /// 已关闭
        /// </summary>
        [Description("已关闭")]
        [Category("1")]
        Closed = 3
    }
}
