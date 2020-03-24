using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZDY.DMS.Domain.Enums
{
    public enum WorkFlowHandleTacticKinds
    {
        /// <summary>
        /// 必须全部同意
        /// </summary>
        [Description("必须全部同意")]
        [Category("1")]
        AllAgree = 0,

        /// <summary>
        /// 一人同意即可
        /// </summary>
        [Description("一人同意即可")]
        [Category("1")]
        OneAgree = 1,

        /// <summary>
        /// 依据人数比例
        /// </summary>
        [Description("依据人数比例")]
        [Category("1")]
        PercentageAgree = 2,

        /// <summary>
        /// 独立处理
        /// </summary>
        [Description("独立处理")]
        [Category("1")]
        Independent = 3
    }
}
