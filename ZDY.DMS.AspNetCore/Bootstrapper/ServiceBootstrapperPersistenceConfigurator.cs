using System;
using ZDY.DMS.Querying.DataTableGateway;
using ZDY.DMS.Repositories;

namespace ZDY.DMS.AspNetCore
{
    public class ServiceBootstrapperPersistenceConfigurator
    {
        private readonly Type serviceModule;

        private readonly ServiceBootstrapper bootstrapper;

        internal ServiceBootstrapperPersistenceConfigurator(Type serviceModule, ServiceBootstrapper bootstrapper)
        {
            this.serviceModule = serviceModule;

            this.bootstrapper = bootstrapper;
        }

        public ServiceBootstrapperPersistenceConfigurator WithRepository(Func<IServiceProvider, IRepositoryContext> repositoryContextFactory)
        {
            if (!this.bootstrapper.ServiceRepositories.TryGetValue(serviceModule, out Func<IServiceProvider, IRepositoryContext> registryItem))
            {
                this.bootstrapper.ServiceRepositories.TryAdd(serviceModule, repositoryContextFactory);
            }

            return this;
        }

        public ServiceBootstrapperPersistenceConfigurator WithDataTableGateway(Func<IServiceProvider, IDataTableGateway> dataTableGatewayFactory)
        {
            if (!this.bootstrapper.ServiceDataTableGatewaies.TryGetValue(serviceModule, out Func<IServiceProvider, IDataTableGateway> registryItem))
            {
                this.bootstrapper.ServiceDataTableGatewaies.TryAdd(serviceModule, dataTableGatewayFactory);
            }

            return this;
        }
    }
}
