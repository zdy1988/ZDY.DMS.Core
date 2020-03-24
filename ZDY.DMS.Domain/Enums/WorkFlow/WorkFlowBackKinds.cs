using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZDY.DMS.Domain.Enums
{
    public enum WorkFlowBackKinds
    {
        /// <summary>
        /// 退回前一步
        /// </summary>
        [Description("退回前一步")]
        [Category("1")]
        ToPrev = 0,

        /// <summary>
        /// 退回第一步
        /// </summary>
        [Description("退回第一步")]
        [Category("1")]
        ToFirst = 1
    }
}
