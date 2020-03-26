using ZDY.DMS.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ZDY.DMS.Events
{
    /// <summary>
    /// Represents the base class for event handlers.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <seealso cref="ZDY.DMS.Messaging.MessageHandler{TEvent}" />
    /// <seealso cref="ZDY.DMS.Events.IEventHandler{TEvent}" />
    public abstract class EventHandler<TEvent> : MessageHandler<TEvent>, IEventHandler<TEvent>
        where TEvent : IEvent
    {

    }
}
