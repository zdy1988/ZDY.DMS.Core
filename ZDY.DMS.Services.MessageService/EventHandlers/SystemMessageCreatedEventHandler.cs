using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.Common.Events;
using ZDY.DMS.Services.MessageService.Enums;
using ZDY.DMS.Services.MessageService.ServiceContracts;

namespace ZDY.DMS.Services.MessageService.EventHandlers
{
    public class SystemMessageCreatedEventHandler : MessageCreatedEventHandler<SystemMessageCreatedEvent>
    {
        public SystemMessageCreatedEventHandler(IMessageInboxService messageInboxService)
            : base(messageInboxService, MessageKinds.System)
        {

        }
    }
}
