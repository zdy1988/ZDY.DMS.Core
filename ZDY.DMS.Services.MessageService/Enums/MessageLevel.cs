using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZDY.DMS.Services.MessageService.Enums
{
    public enum MessageLevel
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        [Category("1")]
        Normal = 0,

        /// <summary>
        /// 提醒
        /// </summary>
        [Description("提醒")]
        [Category("1")]
        Remind = 1,

        /// <summary>
        /// 警告
        /// </summary>
        [Description("警告")]
        [Category("1")]
        Warning = 2,

        /// <summary>
        /// 紧急
        /// </summary>
        [Description("紧急")]
        [Category("1")]
        Urgent = 3
    }
}
