using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZDY.DMS.Domain.Enums
{
    public enum WorkFlowFormState
    {
        /// <summary>
        /// 设计中
        /// </summary>
        [Description("设计中")]
        [Category("1")]
        Designing = 0,

        /// <summary>
        /// 已发布
        /// </summary>
        [Description("已发布")]
        [Category("1")]
        Published = 1,

        /// <summary>
        /// 已删除
        /// </summary>
        [Description("已删除")]
        [Category("1")]
        Deleted = 2
    }
}
