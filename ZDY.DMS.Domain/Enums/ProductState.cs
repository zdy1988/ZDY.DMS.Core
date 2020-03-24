using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZDY.DMS.Domain.Enums
{
    public enum ProductState
    {
        [Description("未上架")]
        [Category("1")]
        Normal = 0,

        [Description("已上架")]
        [Category("1")]
        OnShelf = 1,

        [Description("已下架")]
        [Category("1")]
        OffShelf = 2,

        [Description("已售馨")]
        [Category("1")]
        SellOut = 3
    }
}
