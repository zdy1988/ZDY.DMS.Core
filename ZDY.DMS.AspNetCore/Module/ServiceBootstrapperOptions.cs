using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.AspNetCore.Module;
using ZDY.DMS.Repositories;

namespace ZDY.DMS.AspNetCore
{
    public class ServiceBootstrapperOptions
    {
        private readonly ConcurrentDictionary<Type, Func<IServiceProvider, IRepositoryContext>> serviceModuleRegistrations;

        public ServiceBootstrapperOptions()
        {
            serviceModuleRegistrations = new ConcurrentDictionary<Type, Func<IServiceProvider, IRepositoryContext>>();
        }

        public ConcurrentDictionary<Type, Func<IServiceProvider, IRepositoryContext>> ServiceModules
        {
            get
            {
                return this.serviceModuleRegistrations;
            }
        }

        public ServiceRepositoryBootstrapperOptions AddService(Type serviceModuleType)
        {
            if (!serviceModuleRegistrations.TryGetValue(serviceModuleType, out Func<IServiceProvider, IRepositoryContext> registryItem))
            {
                serviceModuleRegistrations.TryAdd(serviceModuleType, null);
            }

            return new ServiceRepositoryBootstrapperOptions(serviceModuleType, serviceModuleRegistrations);
        }

        public ServiceRepositoryBootstrapperOptions AddService<TService>() where TService : IServiceModule
        {
            return AddService(typeof(TService));
        }
    }
}
