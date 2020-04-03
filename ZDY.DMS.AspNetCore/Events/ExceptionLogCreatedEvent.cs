using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.Events;

namespace ZDY.DMS.AspNetCore.Events
{
    public class ExceptionLogCreatedEvent : Event
    {
        public Guid OperatorId { get; set; }

        public Exception Exception { get; set; }

        public ExceptionLogCreatedEvent(Exception exception, Guid operatorId = default)
        {
            this.OperatorId = operatorId;
            this.Exception = exception;
        }
    }
}
