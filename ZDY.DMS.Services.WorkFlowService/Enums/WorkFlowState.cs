using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZDY.DMS.Services.WorkFlowService.Enums
{
    public enum WorkFlowState
    {
        /// <summary>
        /// 设计中
        /// </summary>
        [Description("设计中")]
        [Category("1")]
        Designing = 0,

        /// <summary>
        /// 已安装
        /// </summary>
        [Description("已安装")]
        [Category("1")]
        Installed = 1,

        /// <summary>
        /// 已删除
        /// </summary>
        [Description("已删除")]
        [Category("1")]
        Deleted = 2
    }
}
