using ZDY.DMS.AspNetCore.Bootstrapper.Module;
using ZDY.DMS.Events;

namespace ZDY.DMS.AspNetCore.Messaging
{
    public abstract class EventHandlerBase<TServiceModule, TEvent> : MessageHandlerBase<TServiceModule, TEvent>
        where TServiceModule : IServiceModule
        where TEvent : IEvent
    {

    }
}
