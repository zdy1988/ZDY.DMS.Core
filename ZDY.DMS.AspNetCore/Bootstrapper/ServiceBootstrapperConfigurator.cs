using System;
using ZDY.DMS.AspNetCore.Bootstrapper.Module;
using ZDY.DMS.Commands;
using ZDY.DMS.Events;
using ZDY.DMS.Repositories;

namespace ZDY.DMS.AspNetCore
{
    public class ServiceBootstrapperConfigurator
    {
        private readonly ServiceBootstrapper bootstrapper;

        internal ServiceBootstrapperConfigurator(ServiceBootstrapper  bootstrapper)
        {
            this.bootstrapper = bootstrapper;
        }

        public ServiceBootstrapperPersistenceConfigurator AddService(Type serviceModule)
        {
            if (!this.bootstrapper.ServiceModules.TryGetValue(serviceModule, out Type registryItem))
            {
                this.bootstrapper.ServiceModules.TryAdd(serviceModule, null);
            }

            return new ServiceBootstrapperPersistenceConfigurator(serviceModule, this.bootstrapper);
        }

        public ServiceBootstrapperPersistenceConfigurator AddService<TService>() where TService : IServiceModule
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
