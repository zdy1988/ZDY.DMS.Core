using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.AspNetCore.Dictionary;
using ZDY.DMS.Commands;
using ZDY.DMS.Events;

namespace ZDY.DMS.AspNetCore.Module
{
    public abstract class ServiceModule : DisposableObject, IServiceModule
    {

        private readonly ICommandSubscriber commandSubscriber;
        private readonly IEventSubscriber eventSubscriber;
        private readonly IDictionaryRegister dictionaryRegister;

        public ServiceModule(IDictionaryRegister dictionaryRegister)
        {
            this.dictionaryRegister = dictionaryRegister;
        }

        public ServiceModule(IDictionaryRegister dictionaryRegister,IEventSubscriber eventSubscriber)
            : this(dictionaryRegister)
        {
            this.eventSubscriber = eventSubscriber;
        }

        public ServiceModule(IDictionaryRegister dictionaryRegister, ICommandSubscriber commandSubscriber, IEventSubscriber eventSubscriber)
            : this(dictionaryRegister, eventSubscriber)
        {
            this.commandSubscriber = commandSubscriber;
        }

        protected ICommandSubscriber CommandSubscriber => this.commandSubscriber;

        protected IEventSubscriber EventSubscriber => this.eventSubscriber;

        protected IDictionaryRegister DictionaryRegister => this.dictionaryRegister;

        protected virtual void CommandHandlersInitializer()
        { }

        protected virtual void EventHandlersInitializer()
        { }

        protected virtual void DictionaryInitializer()
        { }

        public void Initialize()
        {
            CommandHandlersInitializer();
            EventHandlersInitializer();
            DictionaryInitializer();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.CommandSubscriber.Dispose();
                this.EventSubscriber.Dispose();
                this.DictionaryRegister.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
