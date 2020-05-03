using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.Services.Shared.Events
{
    public class SystemMessageCreatedEvent : MessageCreatedEvent
    {
        public SystemMessageCreatedEvent(string title, string content, int level, params Guid[] receiver)
            : base(title, content, level, Guid.Parse("00000000-0000-0000-0000-000000000004"), "系统管理员", receiver)
        {

        }
    }
}
