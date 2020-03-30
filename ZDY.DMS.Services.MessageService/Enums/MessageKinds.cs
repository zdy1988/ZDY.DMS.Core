using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.Services.MessageService.Enums
{
    public enum MessageKinds
    {
        /// <summary>
        /// 私信
        /// </summary>
        Private = 0,

        /// <summary>
        /// 100人以内组消息
        /// </summary>
        Group = 1,

        /// <summary>
        /// 100人以上公共消息
        /// </summary>
        Public = 2,

        /// <summary>
        /// 全员消息
        /// </summary>
        Global = 3,

        /// <summary>
        /// 系统消息
        /// </summary>
        System = 4
    }
}
