using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.Messaging;

namespace ZDY.DMS.Events
{
    /// <summary>
    /// Represents the base class for events.
    /// </summary>
    /// <seealso cref="ZDY.DMS.Messaging.Message" />
    /// <seealso cref="ZDY.DMS.Events.IEvent" />
    public abstract class Event : Message, IEvent
    {
        public const string EventIntentMetadataKey = "$event.intent";
        public const string EventOriginatorClrTypeMetadataKey = "$event.originatorClrtype";
        public const string EventOriginatorIdentifierMetadataKey = "$event.originatorId";

        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
        protected Event()
        {
            Metadata[EventIntentMetadataKey] = this.GetType().Name;
        }

        public string GetEventIntent() => this.Metadata[EventIntentMetadataKey]?.ToString();

        public string GetOriginatorClrType() => this.Metadata[EventOriginatorClrTypeMetadataKey]?.ToString();

        public string GetOriginatorIdentifier() => this.Metadata[EventOriginatorIdentifierMetadataKey]?.ToString();

        public virtual EventDescriptor ToDescriptor()
        {
            return new EventDescriptor
            {
                Id = Guid.NewGuid(),
                EventClrType = this.GetMessageClrTypeName(),
                EventId = this.Id,
                EventIntent = this.GetEventIntent(),
                TimeStamp = this.Timestamp,
                OriginatorClrType = this.GetOriginatorClrType(),
                OriginatorId = this.GetOriginatorIdentifier(),
                EventPayload = this
            };
        }
    }
}
