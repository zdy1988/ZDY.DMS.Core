using System;

namespace ZDY.DMS.Messaging
{
    /// <summary>
    /// Represents that the implemented classes are message subscribers that listen to
    /// the underlying messaging infrastructure and notify the observers when there is
    /// any incoming messages.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IMessageSubscriber : IDisposable
    {
        void Subscribe<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler<TMessage>;

        /// <summary>
        /// Occurs when there is any incoming messages.
        /// </summary>
        event EventHandler<MessageReceivedEventArgs> MessageReceived;

        /// <summary>
        /// Occurs when the message has been acknowledged.
        /// </summary>
        event EventHandler<MessageAcknowledgedEventArgs> MessageAcknowledged;
    }
}
