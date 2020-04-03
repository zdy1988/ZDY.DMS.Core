using System;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.Common.Events;
using ZDY.DMS.Services.MessageService.Enums;
using ZDY.DMS.Services.MessageService.ServiceContracts;

namespace ZDY.DMS.Services.MessageService.EventHandlers
{
    public class GlobalMessageCreatedEventHandler : MessageCreatedEventHandler<GlobalMessageCreatedEvent>
    {
        public GlobalMessageCreatedEventHandler(Func<Type, IRepositoryContext> repositoryContextFactory, IMessageInboxService messageInboxService)
            : base(repositoryContextFactory, messageInboxService, MessageKinds.Global)
        {

        }
    }
}
