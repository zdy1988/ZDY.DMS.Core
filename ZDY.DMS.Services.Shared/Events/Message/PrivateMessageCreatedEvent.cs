using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.Services.Shared.Events
{
    public class PrivateMessageCreatedEvent : MessageCreatedEvent
    {
        public PrivateMessageCreatedEvent(string title, string content, int level, Guid senderId, string senderName, params Guid[] receiver)
            : base(title, content, level, senderId, senderName, receiver)
        {

        }
    }
}
