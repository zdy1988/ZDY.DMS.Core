using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.Events;

namespace ZDY.DMS.Services.Shared.Events
{
    public class MessageCreatedEvent : Event
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public int Level { get; set; }

        public Guid[] Receiver { get; set; }

        public Guid SenderId { get; set; } = default;

        public string SenderName { get; set; } = "No Name";

        public MessageCreatedEvent(string title, string content, int level, params Guid[] receiver)
        {
            this.Title = title;
            this.Content = content;
            this.Level = level;
            this.Receiver = receiver;
        }

        public MessageCreatedEvent(string title, string content, int level, Guid senderId, string senderName, params Guid[] receiver)
              : this(title, content, level, receiver)
        {
            this.SenderId = senderId;
            this.SenderName = senderName;
        }
    }
}
