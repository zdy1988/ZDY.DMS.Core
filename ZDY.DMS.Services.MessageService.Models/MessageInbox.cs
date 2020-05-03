using System;
using System.ComponentModel.DataAnnotations.Schema;
using ZDY.DMS.Services.Shared.Models;

namespace ZDY.DMS.Services.MessageService.Models
{
    [Table("Message_Inbox")]
    public class MessageInbox : BaseEntity
    {
        /// <summary>
        /// 消息Id
        /// </summary>
        public Guid MessageId { get; set; }
        /// <summary>
        /// 接收者Id
        /// </summary>
        public Guid ReceiverId { get; set; }
        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsReaded { get; set; } = false;
    }
}
