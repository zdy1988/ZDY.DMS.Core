using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.Common.Events;
using ZDY.DMS.Services.MessageService.Enums;
using ZDY.DMS.Services.MessageService.ServiceContracts;

namespace ZDY.DMS.Services.MessageService.EventHandlers
{
    public class GroupMessageCreatedEventHandler : MessageCreatedEventHandler<GroupMessageCreatedEvent>
    {
        public GroupMessageCreatedEventHandler(IMessageInboxService messageInboxService)
            : base(messageInboxService, MessageKinds.Group)
        {

        }
    }
}
