using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZDY.DMS.Domain.Enums
{
    /// <summary>
    /// 业务订单类型
    /// </summary>
    public enum BusinessOrderKinds
    {
        /// <summary>
        /// 销售订单
        /// </summary>
        [Description("销售订单")]
        [Category("1")]
        SalesOrder = 0
    }
}
