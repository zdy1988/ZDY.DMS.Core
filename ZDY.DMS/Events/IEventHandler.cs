using ZDY.DMS.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZDY.DMS.Events
{
    /// <summary>
    /// Represents that the implemented classes are event handlers.
    /// </summary>
    /// <seealso cref="ZDY.DMS.Messaging.IMessageHandler" />
    public interface IEventHandler : IMessageHandler
    { }

    /// <summary>
    /// Represents that the implemented classes are event handlers.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event that will be handled by the current handler.</typeparam>
    public interface IEventHandler<in TEvent> : IEventHandler, IMessageHandler<TEvent>
        where TEvent : IEvent
    {
    }
}
