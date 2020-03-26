using System;
using ZDY.DMS.Events;

namespace ZDY.DMS
{
    /// <summary>
    /// Represents the domain event that occurs when an aggregate has been created.
    /// </summary>
    /// <seealso cref="ZDY.DMS.Events.DomainEvent" />
    public sealed class AggregateCreatedEvent<TKey> : DomainEvent
        where TKey : IEquatable<TKey>
    {
        #region Ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateCreatedEvent"/> class.
        /// </summary>
        public AggregateCreatedEvent()
        {

        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateCreatedEvent"/> class.
        /// </summary>
        /// <param name="key">The id of the aggregate root.</param>
        public AggregateCreatedEvent(TKey key)
        {
            this.Key = key;
        }
        #endregion

        /// <summary>
        /// Gets or sets the id of the aggregate root.
        /// </summary>
        /// <value>
        /// The id of the aggregate root.
        /// </value>
        public TKey Key { get; set; }
    }
}
