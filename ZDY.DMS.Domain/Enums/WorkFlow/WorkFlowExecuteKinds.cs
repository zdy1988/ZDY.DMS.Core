using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZDY.DMS.Domain.Enums
{
    public enum WorkFlowExecuteKinds
    {
        /// <summary>
        /// 提交
        /// </summary>
        [Description("提交")]
        [Category("1")]
        Submit = 0,
        /// <summary>
        /// 退回
        /// </summary>
        [Description("退回")]
        [Category("1")]
        Back = 1,
        /// <summary>
        /// 转交
        /// </summary>
        [Description("转交")]
        [Category("1")]
        Redirect = 2
    }
}
