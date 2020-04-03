using System;
using System.Threading;
using System.Threading.Tasks;
using ZDY.DMS.AspNetCore.Messaging;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.Common.Events;
using ZDY.DMS.Services.MessageService.Enums;
using ZDY.DMS.Services.MessageService.Models;
using ZDY.DMS.Services.MessageService.ServiceContracts;

namespace ZDY.DMS.Services.MessageService.EventHandlers
{
    public class MessageCreatedEventHandler<TEvent> : EventHandlerBase<MessageServiceModule, TEvent>
        where TEvent: MessageCreatedEvent
    {
        private readonly IMessageInboxService messageInboxService;

        private readonly MessageKinds messageKinds;

        public MessageCreatedEventHandler(Func<Type, IRepositoryContext> repositoryContextFactory, IMessageInboxService messageInboxService, MessageKinds messageKinds)
            : base(repositoryContextFactory)
        {
            this.messageInboxService = messageInboxService;

            this.messageKinds = messageKinds;
        }

        public async override Task<bool> HandleAsync(TEvent message, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!string.IsNullOrEmpty(message.Content) && message.Receiver.Length > 0)
                {
                    int messageMaxLevel = (int)MessageLevel.Urgent;

                    if (message.Level > messageMaxLevel)
                    {
                        message.Level = messageMaxLevel;
                    }

                    if (string.IsNullOrEmpty(message.Title))
                    {
                        message.Title = message.Content.Length > 15 ? message.Content.Substring(0, 15) : message.Content;
                    }

                    await this.messageInboxService.AddMessageAsync(new Message
                    {
                        Title = message.Title,
                        Content = message.Content,
                        Level = message.Level,
                        Type = (int)messageKinds,
                        SenderId = message.SenderId,
                        SenderName = message.SenderName,
                        IsSended = true
                    }, message.Receiver);
                }
                else
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
