using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using ZDY.DMS.Utilities;

namespace ZDY.DMS.Messaging.Simple
{
    /// <summary>
    /// Represents the message bus that uses a dictionary data structure as the message queue
    /// infrastructure.
    /// </summary>
    /// <seealso cref="ZDY.DMS.DisposableObject" />
    /// <seealso cref="ZDY.DMS.Messaging.IMessageBus" />
    public class SimpleMessageBus : MessageBus
    {
        private readonly MessageQueue messageQueue;

        public SimpleMessageBus(IMessageSerializer messageSerializer,
            IMessageHandlerExecutionContext messageHandlerExecutionContext)
            : base(messageSerializer, messageHandlerExecutionContext)
        {
            this.messageQueue = new MessageQueue();

            InitializeMessageConsumer();
        }

        protected override void DoPublish<TMessage>(TMessage message)
        {
            messageQueue.PushMessage(message);
        }

        protected override Task DoPublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Factory.StartNew(() => DoPublish(message), cancellationToken);
        }

        public override void Subscribe<TMessage, TMessageHandler>()
        {
            this.MessageHandlerExecutionContext.RegisterHandler<TMessage, TMessageHandler>();
        }

        public void InitializeMessageConsumer()
        {
            this.messageQueue.Consume(message =>
            {
                this.OnMessageReceived(new MessageReceivedEventArgs(message, this.MessageSerializer));

                this.MessageHandlerExecutionContext.HandleMessageAsync(message).GetAwaiter().GetResult();

                this.OnMessageAcknowledged(new MessageAcknowledgedEventArgs(message, this.MessageSerializer));
            });
        }
    }
}
