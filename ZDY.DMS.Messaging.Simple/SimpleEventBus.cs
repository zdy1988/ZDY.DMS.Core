using ZDY.DMS.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZDY.DMS.Messaging.Simple
{
    public sealed class SimpleEventBus : SimpleMessageBus, IEventBus
    {
        public SimpleEventBus(IMessageSerializer messageSerializer, IMessageHandlerExecutionContext messageHandlerExecutionContext)
            : base(messageSerializer, messageHandlerExecutionContext)
        { }
    }
}
