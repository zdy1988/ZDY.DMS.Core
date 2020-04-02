using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.AspNetCore.Dictionary;
using ZDY.DMS.AspNetCore.EntityMapper;
using ZDY.DMS.Commands;
using ZDY.DMS.Events;

namespace ZDY.DMS.AspNetCore.Module
{
    public abstract class ServiceModule : DisposableObject, IServiceModule
    {

        private readonly ICommandSubscriber commandSubscriber;
        private readonly IEventSubscriber eventSubscriber;
        private readonly IDictionaryRegister dictionaryRegister;
        private readonly IEntityMapperRegister entityMapperRegister;

        public ServiceModule(IDictionaryRegister dictionaryRegister)
        {
            this.dictionaryRegister = dictionaryRegister;
        }

        public ServiceModule(IDictionaryRegister dictionaryRegister, IEntityMapperRegister entityMapperRegister)
              : this(dictionaryRegister)
        {
            this.entityMapperRegister = entityMapperRegister;
        }

        public ServiceModule(IDictionaryRegister dictionaryRegister, IEntityMapperRegister entityMapperRegister, IEventSubscriber eventSubscriber)
            : this(dictionaryRegister, entityMapperRegister)
        {
            this.eventSubscriber = eventSubscriber;
        }

        public ServiceModule(IDictionaryRegister dictionaryRegister, IEntityMapperRegister entityMapperRegister, ICommandSubscriber commandSubscriber, IEventSubscriber eventSubscriber)
            : this(dictionaryRegister, entityMapperRegister, eventSubscriber)
        {
            this.commandSubscriber = commandSubscriber;
        }

        protected ICommandSubscriber CommandSubscriber => this.commandSubscriber;

        protected IEventSubscriber EventSubscriber => this.eventSubscriber;

        protected IDictionaryRegister DictionaryRegister => this.dictionaryRegister;

        protected IEntityMapperRegister EntityMapperRegister => this.entityMapperRegister;

        protected virtual void CommandHandlersInitializer()
        { }

        protected virtual void EventHandlersInitializer()
        { }

        protected virtual void DictionaryInitializer()
        { }

        protected virtual void EntityMapperInitializer()
        { }

        public void Initialize()
        {
            CommandHandlersInitializer();
            EventHandlersInitializer();
            DictionaryInitializer();
            EntityMapperInitializer();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.CommandSubscriber.Dispose();
                this.EventSubscriber.Dispose();
                this.DictionaryRegister.Dispose();
                this.EntityMapperRegister.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
