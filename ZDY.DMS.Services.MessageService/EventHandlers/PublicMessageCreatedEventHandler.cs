using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.Common.Events;
using ZDY.DMS.Services.MessageService.Enums;
using ZDY.DMS.Services.MessageService.ServiceContracts;

namespace ZDY.DMS.Services.MessageService.EventHandlers
{
    public class PublicMessageCreatedEventHandler : MessageCreatedEventHandler<PublicMessageCreatedEvent>
    {
        public PublicMessageCreatedEventHandler(Func<Type, IRepositoryContext> repositoryContextFactory, IMessageInboxService messageInboxService)
            : base(repositoryContextFactory, messageInboxService, MessageKinds.Public)
        {

        }
    }
}
