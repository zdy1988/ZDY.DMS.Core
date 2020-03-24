using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZDY.DMS.Domain.Enums
{
    public enum WarehouseOrderState
    {
        /// <summary>
        /// 已提交
        /// </summary>
        [Description("已提交")]
        [Category("1")]
        Submited = 0,

        /// <summary>
        /// 已确认
        /// </summary>
        [Description("已确认")]
        [Category("1")]
        Confirmed = 1
    }
}
