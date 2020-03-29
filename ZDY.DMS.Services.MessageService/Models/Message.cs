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
        /// 跳转的地址
        /// </summary>
        public string RedirectUrl { get; set; }
        /// <summary>
        /// 推送时间
        /// </summary>
        public DateTime PushTime { get; set; }
        /// <summary>
        /// 操作创建人
        /// </summary>
        public Guid OperatorId { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// 类型 0 private私信，1 group 100人以内组消息，2 public 100人以上公共消息，3 global 全员消息
        /// </summary>
        public int Type { get; set; } = 0;
        /// <summary>
        /// 等级 0 正常 1 注意 2 提醒 3 警告 4 紧急
        /// </summary>
        public int Level { get; set; } = 0;
        /// <summary>
        ///  状态 0 草稿 1 已发送
        /// </summary>
        public int State { get; set; } = 0;
    }
}
