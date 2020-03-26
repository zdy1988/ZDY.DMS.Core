using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZDY.DMS.Messaging
{
    /// <summary>
    /// Represents the message handler execution context, in which the message handlers are going
    /// to handle the received messages in a particular context, so that the dependent resources
    /// can be easily managed and maintained.
    /// </summary>
    public interface IMessageHandlerExecutionContext
    {
        /// <summary>
        /// Registers the message handler to the current execution context.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message whose handler is going to be registered.</typeparam>
        /// <typeparam name="THandler">The type of the message handler that is going to be registered.</typeparam>
        void RegisterHandler<TMessage, THandler>()
            where TMessage : IMessage
            where THandler : IMessageHandler<TMessage>;

        /// <summary>
        /// Checks whether the message handler of the specified message type has been registered.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message whose handler's existence should be checked.</typeparam>
        /// <typeparam name="THandler">The type of the message handler whose existence should be checked.</typeparam>
        /// <returns></returns>
        bool HandlerRegistered<TMessage, THandler>()
            where TMessage : IMessage
            where THandler : IMessageHandler<TMessage>;

        void RegisterHandler(Type messageType, Type handlerType);

        bool HandlerRegistered(Type messageType, Type handlerType);

        Task HandleMessageAsync(IMessage message, CancellationToken cancellationToken = default(CancellationToken));
    }
}
