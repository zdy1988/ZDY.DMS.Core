using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ZDY.DMS.Messaging
{
    /// <summary>
    /// Represents that the implemented classes are message publishers that can
    /// publish the specified message to a message bus.
    /// </summary>
    public interface IMessagePublisher : IDisposable
    {
        /// <summary>
        /// Publishes the specified message to the message bus.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message to be published.</typeparam>
        /// <param name="message">The message that is going to be published.</param>
        /// <param name="route">The routing of the message publication. In some of the message publisher implementation,
        /// this parameter can be ignored.</param>
        void Publish<TMessage>(TMessage message)
            where TMessage : IMessage;

        /// <summary>
        /// Publishes all the messages to the message bus.
        /// </summary>
        /// <param name="messages">The messages that is going to be published.</param>
        /// <param name="route">The routing of the message publication. In some of the message publisher implementation,
        /// this parameter can be ignored.</param>
        void PublishAll(IEnumerable<IMessage> messages);

        /// <summary>
        /// Publishes the specified message to the message bus asynchronously.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message to be published.</typeparam>
        /// <param name="message">The message that is going to be published.</param>
        /// <param name="route">The routing of the message publication. In some of the message publisher implementation,
        /// this parameter can be ignored.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> instance that propagates notification that operations should be canceled.</param>
        /// <returns>The <see cref="Task"/> that executes the message publication.</returns>
        Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default(CancellationToken))
            where TMessage : IMessage;

        /// <summary>
        /// Publishes all the messages to the message bus asynchronously.
        /// </summary>
        /// <param name="messages">The messages that is going to be published.</param>
        /// <param name="route">The routing of the message publication. In some of the message publisher implementation,
        /// this parameter can be ignored.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> which propagates notification that operations should be canceled.</param>
        /// <returns>The <see cref="Task"/> that executes the message publication.</returns>
        Task PublishAllAsync(IEnumerable<IMessage> messages, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Represents the event that occurs when the message has been published.
        /// </summary>
        event EventHandler<MessagePublishedEventArgs> MessagePublished;
    }
}
