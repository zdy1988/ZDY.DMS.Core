using System;
using ZDY.DMS.AspNetCore.Module;
using ZDY.DMS.Commands;
using ZDY.DMS.Events;
using ZDY.DMS.Repositories;

namespace ZDY.DMS.AspNetCore
{
    public class ServiceBootstrapperConfigurator
    {
        private readonly ServiceBootstrapper bootstrapper;

        public ServiceBootstrapperConfigurator(ServiceBootstrapper  bootstrapper)
        {
            this.bootstrapper = bootstrapper;
        }

        public ServiceRepositoryBootstrapperConfigurator AddService(Type serviceModuleType)
        {
            if (!this.bootstrapper.ServiceModules.TryGetValue(serviceModuleType, out Func<IServiceProvider, IRepositoryContext> registryItem))
            {
                this.bootstrapper.ServiceModules.TryAdd(serviceModuleType, null);
            }

            return new ServiceRepositoryBootstrapperConfigurator(serviceModuleType, this.bootstrapper);
        }

        public ServiceRepositoryBootstrapperConfigurator AddService<TService>() where TService : IServiceModule
        {
            return AddService(typeof(TService));
        }

        public void UseEventBus(Func<IServiceProvider, IEventBus> eventBusFactory)
        {
            this.bootstrapper.AddEventBus(eventBusFactory);
        }

        public void UseEventBus()
        {
            this.bootstrapper.AddEventBus();
        }

        public void UseCommandBus(Func<IServiceProvider, ICommandBus> commandBusFactory)
        {
            this.bootstrapper.AddCommandBus(commandBusFactory);
        }

        public void UseCommandBus()
        {
            this.bootstrapper.AddCommandBus();
        }

        public void UseEventStore(Func<IServiceProvider, IEventStore> eventStoreFactory)
        {
            this.bootstrapper.AddEventStore(eventStoreFactory);
        }

        public void UseEventStore(string eventStoreConnectionString)
        {
            this.bootstrapper.AddEventStore(eventStoreConnectionString);
        }
    }
}
