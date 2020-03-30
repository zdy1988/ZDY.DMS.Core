using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZDY.DMS.Services.AdminService.Enums
{
    /// <summary>
    /// 字典的种类
    /// </summary>
    public enum DictionaryKinds
    {
        /// <summary>
        /// Sql语句
        /// </summary>
        [Description("Sql语句")]
        [Category("1")]
        SqlQuery = 1,

        /// <summary>
        /// 枚举
        /// </summary>
        [Description("枚举")]
        [Category("1")]
        Enum = 2,

        /// <summary>
        /// 键值
        /// </summary>
        [Description("键值")]
        [Category("1")]
        KeyVlaue = 3,

        /// <summary>
        /// 方法
        /// </summary>
        [Description("方法")]
        [Category("1")]
        Method = 4
    }
}
