using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.AspNetCore.Bootstrapper.Module;
using ZDY.DMS.Events;
using ZDY.DMS.Repositories;

namespace ZDY.DMS.AspNetCore.Messaging
{
    public abstract class EventHandlerBase<TServiceModule, TEvent> : MessageHandlerBase<TServiceModule, TEvent>
        where TServiceModule : IServiceModule
        where TEvent : IEvent
    {

        public EventHandlerBase(Func<Type, IRepositoryContext> repositoryContextFactory)
            : base(repositoryContextFactory)
        {

        }
    }
}
