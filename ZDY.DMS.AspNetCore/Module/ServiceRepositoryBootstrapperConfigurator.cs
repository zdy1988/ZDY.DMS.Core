using System;
using ZDY.DMS.Repositories;

namespace ZDY.DMS.AspNetCore
{
    public class ServiceRepositoryBootstrapperConfigurator
    {
        private readonly Type serviceModuleType;

        private readonly ServiceBootstrapper bootstrapper;

        public ServiceRepositoryBootstrapperConfigurator(Type serviceModuleType, ServiceBootstrapper bootstrapper)
        {
            this.serviceModuleType = serviceModuleType;

            this.bootstrapper = bootstrapper;
        }

        public void WithRepository(Func<IServiceProvider, IRepositoryContext> repositoryContextFactory)
        {
            if (this.bootstrapper.ServiceModules.TryGetValue(serviceModuleType, out Func<IServiceProvider, IRepositoryContext> registryRepositoryContextFactory))
            {
                if (registryRepositoryContextFactory == null)
                {
                    this.bootstrapper.ServiceModules[serviceModuleType] = repositoryContextFactory;
                }
            }
        }
    }
}
