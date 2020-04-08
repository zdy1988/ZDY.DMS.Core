using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ZDY.DMS.AspNetCore.Messaging;
using ZDY.DMS.AspNetCore.Bootstrapper.Module;
using ZDY.DMS.Commands;
using ZDY.DMS.Events;
using ZDY.DMS.EventStore.AdoNet;
using ZDY.DMS.EventStore.MySQL;
using ZDY.DMS.KeyGeneration;
using ZDY.DMS.Messaging.Simple;
using ZDY.DMS.Repositories;
using ZDY.DMS.Serialization.Json;
using ZDY.DMS.Snapshots;
using ZDY.DMS.Querying.DataTableGateway;

namespace ZDY.DMS.AspNetCore
{
    public class ServiceBootstrapper
    {
        private readonly IServiceCollection services;

        private readonly ServiceBootstrapperConfigurator config;

        private readonly ConcurrentDictionary<Type, Type> serviceModuleRegistrations;

        private readonly ConcurrentDictionary<Type, Func<IServiceProvider, IRepositoryContext>> serviceRepositoryRegistrations;

        private readonly ConcurrentDictionary<Type, Func<IServiceProvider, IDataTableGateway>> serviceDataTableGatewayRegistrations;

        internal ServiceBootstrapper(IServiceCollection services)
        {
            this.services = services;

            this.config = new ServiceBootstrapperConfigurator(this);

            this.serviceModuleRegistrations = new ConcurrentDictionary<Type, Type>();
                  
            this.serviceRepositoryRegistrations = new ConcurrentDictionary<Type, Func<IServiceProvider, IRepositoryContext>>();

            this.serviceDataTableGatewayRegistrations = new ConcurrentDictionary<Type, Func<IServiceProvider, IDataTableGateway>>();
        }

        internal ConcurrentDictionary<Type, Type> ServiceModules
        {
            get
            {
                return this.serviceModuleRegistrations;
            }
        }

        internal ConcurrentDictionary<Type, Func<IServiceProvider, IRepositoryContext>> ServiceRepositories
        {
            get
            {
                return this.serviceRepositoryRegistrations;
            }
        }

        internal ConcurrentDictionary<Type, Func<IServiceProvider, IDataTableGateway>> ServiceDataTableGatewaies
        {
            get
            {
                return this.serviceDataTableGatewayRegistrations;
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
                services.AddScoped(repositoryContextFactory =>
                {
                    IRepositoryContext factory(Type serviceModule)
                    {
                        if (serviceRepositoryRegistrations.TryGetValue(serviceModule, out Func<IServiceProvider, IRepositoryContext> registryRepositoryContextFactory))
                        {
                            return registryRepositoryContextFactory.Invoke(repositoryContextFactory);
                        }

                        throw new InvalidOperationException($"Unable to resolve repository context for module '{serviceModule}'");
                    }

                    return (Func<Type, IRepositoryContext>)factory;
                });

                // Register DataTableGateway Owner Of The Module
                services.AddScoped(dataTableGatewayFactory =>
                {
                    IDataTableGateway factory(Type serviceModule)
                    {
                        if (serviceDataTableGatewayRegistrations.TryGetValue(serviceModule, out Func<IServiceProvider, IDataTableGateway> registryDataTableGatewayFactory))
                        {
                            return registryDataTableGatewayFactory.Invoke(dataTableGatewayFactory);
                        }

                        throw new InvalidOperationException($"Unable to resolve data table gateway for module '{serviceModule}'");
                    }

                    return (Func<Type, IDataTableGateway>)factory;
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
