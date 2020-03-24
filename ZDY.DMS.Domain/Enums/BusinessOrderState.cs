using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZDY.DMS.Domain.Enums
{
    /// <summary>
    /// 业务订单状态
    /// </summary>
    public enum BusinessOrderState
    {
        /// <summary>
        /// 未配货
        /// </summary>
        [Description("未配货")]
        [Category("1")]
        NoPrepare = 0 ,

        /// <summary>
        /// 已配货
        /// </summary>
        [Description("已配货")]
        [Category("1")]
        Prepared = 1,

        /// <summary>
        /// 已发货
        /// </summary>
        [Description("已发货")]
        [Category("1")]
        Delivery = 2,

        /// <summary>
        /// 已丢失
        /// </summary>
        [Description("已丢失")]
        [Category("1")]
        Loss = 5
    }
}
