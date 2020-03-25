using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZDY.DMS.Domain.Enums
{
    public enum WorkFlowCountersignatureTacticKinds
    {
        /// <summary>
        /// 当前步骤禁止使用会签
        /// </summary>
        [Description("当前步骤禁止使用会签")]
        [Category("1")]
        NoCountersignature = 0,

        /// <summary>
        /// 必须同意之前所有步骤
        /// </summary>
        [Description("必须同意之前所有步骤")]
        [Category("1")]
        AllAgree = 1,

        /// <summary>
        /// 同意之前一个步骤即可
        /// </summary>
        [Description("同意之前一个步骤即可")]
        [Category("1")]
        OneAgree = 2,

        /// <summary>
        /// 依据之前步骤同意比例
        /// </summary>
        [Description("依据之前步骤同意比例")]
        [Category("1")]
        PercentageAgree = 3

    }
}
