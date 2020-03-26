namespace ZDY.DMS.Messaging
{
    /// <summary>
    /// Represents the event data that is generated when the message has been acknowledged.
    /// </summary>
    /// <seealso cref="ZDY.DMS.Messaging.MessageProcessedEventArgs" />
    public sealed class MessageAcknowledgedEventArgs : MessageProcessedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageAcknowledgedEventArgs"/> class.
        /// </summary>
        /// <param name="message">The message that has been processed.</param>
        /// <param name="messageSerializer"></param>
        public MessageAcknowledgedEventArgs(IMessage message, IMessageSerializer messageSerializer)
            : this(message, messageSerializer, false)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageAcknowledgedEventArgs"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageSerializer">The message serializer.</param>
        /// <param name="autoAck">if set to <c>true</c> [automatic ack].</param>
        public MessageAcknowledgedEventArgs(IMessage message, IMessageSerializer messageSerializer, bool autoAck) 
            : base(message, messageSerializer)
        {
            this.AutoAck = autoAck;
        }

        /// <summary>
        /// Gets a value indicating whether the message has been acknowledged.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the message has been acknowledged; otherwise, <c>false</c>.
        /// </value>
        public bool AutoAck { get; }
    }
}
