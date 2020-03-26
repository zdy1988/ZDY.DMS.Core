using ZDY.DMS.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZDY.DMS.Messaging.Simple
{
    public sealed class SimpleCommandBus : SimpleMessageBus, ICommandBus
    {
        public SimpleCommandBus(IMessageSerializer messageSerializer, IMessageHandlerExecutionContext messageHandlerExecutionContext)
            : base(messageSerializer, messageHandlerExecutionContext)
        { }
    }
}
