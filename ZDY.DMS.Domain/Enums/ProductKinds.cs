using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZDY.DMS.Domain.Enums
{
    public enum ProductKinds
    {
        /// <summary>
        /// 服装
        /// </summary>
        [Description("服装")]
        [Category("1")]
        Garments = 0,

        /// <summary>
        /// 鞋材
        /// </summary>
        [Description("鞋材")]
        [Category("1")]
        Shoes = 1,

        /// <summary>
        /// 窗帘
        /// </summary>
        [Description("窗帘")]
        [Category("1")]
        Curtains = 2,

        /// <summary>
        /// 工艺品
        /// </summary>
        [Description("工艺品")]
        [Category("1")]
        Crafts = 3,

        /// <summary>
        /// 箱包
        /// </summary>
        [Description("箱包")]
        [Category("1")]
        Bags = 4,

        /// <summary>
        /// 饰品
        /// </summary>
        [Description("饰品")]
        [Category("1")]
        Accessories = 5,

        /// <summary>
        /// 饰品
        /// </summary>
        [Description("内衣")]
        [Category("1")]
        Underwear = 6
    }
}
