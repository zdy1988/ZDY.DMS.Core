using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.AspNetCore.Dictionary;
using ZDY.DMS.AspNetCore.Events;
using ZDY.DMS.AspNetCore.Module;
using ZDY.DMS.Events;
using ZDY.DMS.Services.AdminService.Enums;
using ZDY.DMS.Services.AdminService.EventHandlers;
using ZDY.DMS.Services.Common.Events;

namespace ZDY.DMS.Services.AdminService
{
    public class AdminServiceModule : ServiceModule
    {
        public AdminServiceModule(IDictionaryRegister dictionaryRegister, IEventSubscriber eventSubscriber)
            : base(dictionaryRegister, eventSubscriber)
        {

        }

        protected override void DictionaryInitializer()
        {
            this.DictionaryRegister.RegisterEnum<DictionaryKinds>();
            this.DictionaryRegister.RegisterEnum<LogKinds>();
        }

        protected override void EventHandlersInitializer()
        {
            this.EventSubscriber.Subscribe<WorkFlowInstalledEvent, WorkFlowInstalledEventHandler>();
            this.EventSubscriber.Subscribe<WorkFlowUnInstalledEvent, WorkFlowUnInstalledEventHandler>();

            this.EventSubscriber.Subscribe<ExceptionLogCreatedEvent, ExceptionLogCreatedEventHandler>();
        }
    }
}
