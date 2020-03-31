using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.Repositories;

namespace ZDY.DMS.AspNetCore
{
    public class ServiceRepositoryBootstrapperOptions
    {
        private readonly Type serviceModuleType;

        private readonly ConcurrentDictionary<Type, Func<IServiceProvider, IRepositoryContext>> serviceRegistrations;

        public ServiceRepositoryBootstrapperOptions(Type serviceModuleType, ConcurrentDictionary<Type, Func<IServiceProvider, IRepositoryContext>> serviceRegistrations)
        {
            this.serviceModuleType = serviceModuleType;

            this.serviceRegistrations = serviceRegistrations;
        }

        public void WithRepository(Func<IServiceProvider, IRepositoryContext> repositoryContextFactory)
        {
            if (serviceRegistrations.TryGetValue(serviceModuleType, out Func<IServiceProvider, IRepositoryContext> registryRepositoryContextFactory))
            {
                if (registryRepositoryContextFactory == null)
                {
                    serviceRegistrations[serviceModuleType] = repositoryContextFactory;
                }
            }
        }
    }
}
