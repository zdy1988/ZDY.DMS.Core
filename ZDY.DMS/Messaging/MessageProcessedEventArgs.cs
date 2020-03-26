using System;

namespace ZDY.DMS.Messaging
{
    /// <summary>
    /// Represents the event data that is generated when the message has been processed.
    /// </summary>
    public class MessageProcessedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessedEventArgs"/> class.
        /// </summary>
        /// <param name="message">The message that has been processed.</param>
        public MessageProcessedEventArgs(IMessage message, IMessageSerializer messageSerializer)
        {
            this.Message = message;
            this.MessageSerializer = messageSerializer;
        }

        /// <summary>
        /// Gets the message that has been processed.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public IMessage Message { get; }

        /// <summary>
        /// Gets the instance of the message serializer that is able to serialize or deserialize
        /// the message object carried by this event data.
        /// </summary>
        public IMessageSerializer MessageSerializer { get; }
    }
}
