using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZDY.DMS.Messaging
{
    /// <summary>
    /// Represents that the implemented classes are message handlers.
    /// </summary>
    public interface IMessageHandler
    {
        /// <summary>
        /// Determines whether the current message handler can handle the message with the specified message type.
        /// </summary>
        /// <param name="messageType">Type of the message to be checked.</param>
        /// <returns>
        ///   <c>true</c> if the current message handler can handle the message with the specified message type; otherwise, <c>false</c>.
        /// </returns>
        bool CanHandle(Type messageType);

        /// <summary>
        /// Handles the specified message asynchronously.
        /// </summary>
        /// <param name="message">The message to be handled by the current message handler.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> which propagates notification that operations should be canceled..</param>
        /// <returns>The message handling result. <c>true</c> if the message has been handled successfully, otherwise, <c>false</c>.</returns>
        Task<bool> HandleAsync(IMessage message, CancellationToken cancellationToken = default(CancellationToken));
    }

    /// <summary>
    /// Represents that the implemented classes are message handlers.
    /// </summary>
    /// <typeparam name="TMessage">The type of the message to be handled by current handler.</typeparam>
    public interface IMessageHandler<in TMessage> : IMessageHandler
        where TMessage : IMessage
    {
        /// <summary>
        /// Handles the specified message asynchronously.
        /// </summary>
        /// <param name="message">The message to be handled.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> instance which propagates notification that operations should be canceled.</param>
        /// <returns><c>true</c> if the message has been handled properly, otherwise, <c>false</c>.</returns>
        Task<bool> HandleAsync(TMessage message, CancellationToken cancellationToken = default(CancellationToken));
    }
}
