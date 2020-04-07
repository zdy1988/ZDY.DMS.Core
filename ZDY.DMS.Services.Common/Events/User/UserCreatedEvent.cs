using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.Events;

namespace ZDY.DMS.Services.Common.Events
{
    public class UserCreatedEvent : Event
    {
        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string EncryptedPassword { get; set; }

        public Guid ConpanyId { get; set; }

        public UserCreatedEvent(Guid userId, string name, string encryptedPassword,Guid conpanyId)
        {
            this.UserId = userId;
            this.Name = name;
            this.EncryptedPassword = encryptedPassword;
            this.ConpanyId = conpanyId;
        }
    }
}
