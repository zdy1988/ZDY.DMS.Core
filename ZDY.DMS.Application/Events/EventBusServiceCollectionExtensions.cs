using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Zdy.Events;
using ZDY.DMS.Domain.EventHandlers;
using ZDY.DMS.Domain.Events;

namespace ZDY.DMS.Application.AutoMapper
{
    public static class EventBusServiceCollectionExtensions
    {
        public static void AddEventBus(this IServiceCollection services)
        {
            services.AddSingleton<IEventAggregator, EventAggregator>();
            services.AddTransient<IEventBus, EventBus>();
            
            //事件
            services.AddTransient<IEventHandler<LoggingEvent>, LoggingEventHandler>();
            services.AddTransient<IEventHandler<SendEmailEvent>, SendEmailEventHandler>();
            services.AddTransient<IEventHandler<SendMessageEvent>, SendMessageEventHandler>();
        }
    }
}
