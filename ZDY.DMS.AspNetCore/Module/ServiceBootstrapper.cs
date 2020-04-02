﻿using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ZDY.DMS.AspNetCore.Messaging;
using ZDY.DMS.AspNetCore.Module;
using ZDY.DMS.Commands;
using ZDY.DMS.Events;
using ZDY.DMS.EventStore.AdoNet;
using ZDY.DMS.EventStore.MySQL;
using ZDY.DMS.KeyGeneration;
using ZDY.DMS.Messaging.Simple;
using ZDY.DMS.Repositories;
using ZDY.DMS.Serialization.Json;
using ZDY.DMS.Snapshots;

namespace ZDY.DMS.AspNetCore
{
    public class ServiceBootstrapper
    {
        private readonly IServiceCollection services;

        private readonly ServiceBootstrapperConfigurator config;

        private readonly ConcurrentDictionary<Type, Func<IServiceProvider, IRepositoryContext>> serviceModuleRegistrations;

        public ServiceBootstrapper(IServiceCollection services)
        {
            this.services = services;

            this.config = new ServiceBootstrapperConfigurator(this);

            this.serviceModuleRegistrations = new ConcurrentDictionary<Type, Func<IServiceProvider, IRepositoryContext>>();
        }

        internal ConcurrentDictionary<Type, Func<IServiceProvider, IRepositoryContext>> ServiceModules
        {
            get
            {
                return this.serviceModuleRegistrations;
            }
        }

        internal void Configure(Action<ServiceBootstrapperConfigurator> configure = null)
        {
            configure?.Invoke(config);

            if (serviceModuleRegistrations.Count > 0)
            {
                // Register Modules
                foreach (var module in serviceModuleRegistrations)
                {
                    services.AddSingleton(module.Key);
                }

                // Register RepositoryContext Owner Of The Module
                services.AddScoped(implementationFactory =>
                {
                    IRepositoryContext factory(Type serviceModuleType)
                    {
                        if (serviceModuleRegistrations.TryGetValue(serviceModuleType, out Func<IServiceProvider, IRepositoryContext> registryRepositoryContextFactory))
                        {
                            return registryRepositoryContextFactory.Invoke(implementationFactory);
                        }

                        throw new InvalidOperationException($"Unable to resolve repository context for module '{serviceModuleType}'");
                    }

                    return (Func<Type, IRepositoryContext>)factory;
                });
            }
        }

        internal void Initialize(IApplicationBuilder builder)
        {
            if (serviceModuleRegistrations.Count > 0)
            {
                foreach (var item in serviceModuleRegistrations)
                {
                    IServiceModule module = (IServiceModule)builder.ApplicationServices.GetService(item.Key);

                    module?.Initialize();
                }
            }
        }

        internal void AddEventBus(Func<IServiceProvider, IEventBus> eventBusFactory)
        {
            services.AddSingleton<IEventSubscriber>(eventBusFactory);
            services.AddTransient<IEventPublisher>(eventBusFactory);
        }

        internal void AddEventBus()
        {
            var messageHandlerExecutionContext = new ServiceProviderMessageHandlerExecutionContext(services, sp => sp.BuildServiceProvider());
            var messageSerializer = new MessageJsonSerializer();

            AddEventBus(sp => new SimpleEventBus(messageSerializer, messageHandlerExecutionContext));
        }

        internal void AddCommandBus(Func<IServiceProvider, ICommandBus> commandBusFactory)
        {
            services.AddSingleton<ICommandSender>(commandBusFactory);
            services.AddSingleton<ICommandSubscriber>(commandBusFactory);
        }

        internal void AddCommandBus()
        {
            var messageHandlerExecutionContext = new ServiceProviderMessageHandlerExecutionContext(services, sp => sp.BuildServiceProvider());
            var messageSerializer = new MessageJsonSerializer();

            AddCommandBus(sp => new SimpleCommandBus(messageSerializer, messageHandlerExecutionContext));
        }

        internal void AddEventStore(Func<IServiceProvider, IEventStore> eventStoreFactory)
        {
            services.AddTransient<IEventStore>(eventStoreFactory);
            services.AddSingleton<ISnapshotProvider, SuppressedSnapshotProvider>();
            services.AddTransient<IDomainRepository, EventSourcingDomainRepository>();
        }

        internal void AddEventStore(string eventStoreConnectionString)
        {
            if (string.IsNullOrEmpty(eventStoreConnectionString))
            {
                throw new ArgumentNullException(eventStoreConnectionString);
            }

            var objectJsonSerializer = new ObjectJsonSerializer();

            var config = new AdoNetEventStoreConfiguration(eventStoreConnectionString, new GuidKeyGenerator());

            AddEventStore(sp => new MySqlEventStore(config, objectJsonSerializer));
        }
    }
}
