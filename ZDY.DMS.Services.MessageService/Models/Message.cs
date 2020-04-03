using System;
using ZDY.DMS.Services.Common.Models;

namespace ZDY.DMS.Services.MessageService.Models
{
    public class Message : BaseEntity
    {
        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 操作创建人
        /// </summary>
        public Guid SenderId { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>
        public string SenderName { get; set; }
        /// <summary>
        /// 类型 0 private私信，1 group 组内消息，2 public 公共消息，3 global 全员消息
        /// </summary>
        public int Type { get; set; } = 0;
        /// <summary>
        /// 等级 0 正常的 1 关键的 2 提醒的 3 警告的 4 紧急的
        /// </summary>
        public int Level { get; set; } = 0;
        /// <summary>
        /// 是否已发送
        /// </summary>
        public bool IsSended { get; set; }
    }
}
