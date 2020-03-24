using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZDY.DMS.Domain.Enums
{
    /// <summary>
    /// 仓库单据类型
    /// </summary>
    public enum WarehouseOrderKinds
    {
        /// <summary>
        /// 入库单
        /// </summary>
        [Description("入库单")]
        [Category("1")]
        Entry = 0,

        /// <summary>
        /// 出库单
        /// </summary>
        [Description("出库单")]
        [Category("1")]
        Delivery = 1,

        /// <summary>
        /// 退库单，相当于出库给厂商
        /// </summary>
        [Description("退库单")]
        [Category("1")]
        Refund = 2,

        /// <summary>
        /// 返货单，相当于客户返货后入库
        /// </summary>
        [Description("返货单")]
        [Category("1")]
        Return = 3,

        /// <summary>
        /// 调整单,相当于盘点调整
        /// </summary>
        [Description("调整单")]
        [Category("1")]
        Adjust = 4
    }
}
