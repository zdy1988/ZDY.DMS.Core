using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZDY.DMS.Services.MessageService.Enums
{
    public enum MessageKinds
    {
        /// <summary>
        /// 私信
        /// </summary>
        [Description("私信")]
        [Category("1")]
        Private = 0,

        /// <summary>
        /// 组内消息
        /// </summary>
        [Description("组内消息")]
        [Category("1")]
        Group = 1,

        /// <summary>
        /// 公共消息
        /// </summary>
        [Description("公共消息")]
        [Category("1")]
        Public = 2,

        /// <summary>
        /// 全局消息
        /// </summary>
        [Description("全局消息")]
        [Category("1")]
        Global = 3,

        /// <summary>
        /// 系统消息
        /// </summary>
        [Description("系统消息")]
        [Category("1")]
        System = 4
    }
}
