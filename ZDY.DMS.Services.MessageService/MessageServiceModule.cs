using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.AspNetCore.Dictionary;
using ZDY.DMS.AspNetCore.Module;
using ZDY.DMS.Events;
using ZDY.DMS.Services.Common.Events;
using ZDY.DMS.Services.MessageService.Enums;
using ZDY.DMS.Services.MessageService.EventHandlers;

namespace ZDY.DMS.Services.MessageService
{
    public class MessageServiceModule  : ServiceModule
    {
        public MessageServiceModule(IDictionaryRegister dictionaryRegister, IEventSubscriber eventSubscriber)
            : base(dictionaryRegister, eventSubscriber)
        {

        }

        protected override void DictionaryInitializer()
        {
            this.DictionaryRegister.RegisterEnum<MessageKinds>();
            this.DictionaryRegister.RegisterEnum<MessageLevel>();
        }

        protected override void EventHandlersInitializer()
        {
            this.EventSubscriber.Subscribe<PrivateMessageCreatedEvent, PrivateMessageCreatedEventHandler>();
            this.EventSubscriber.Subscribe<GroupMessageCreatedEvent, GroupMessageCreatedEventHandler>();
            this.EventSubscriber.Subscribe<PublicMessageCreatedEvent, PublicMessageCreatedEventHandler>();
            this.EventSubscriber.Subscribe<GlobalMessageCreatedEvent, GlobalMessageCreatedEventHandler>();
            this.EventSubscriber.Subscribe<SystemMessageCreatedEvent, SystemMessageCreatedEventHandler>();
        }
    }
}
