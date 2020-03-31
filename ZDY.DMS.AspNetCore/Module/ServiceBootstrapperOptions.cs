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
        private readonly ConcurrentDictionary<Type, Func<IServiceProvider, IRepositoryContext>> serviceRegistrations;

        public ServiceBootstrapperOptions()
        {
            serviceRegistrations = new ConcurrentDictionary<Type, Func<IServiceProvider, IRepositoryContext>>();
        }

        public ConcurrentDictionary<Type, Func<IServiceProvider, IRepositoryContext>> Services
        {
            get
            {
                return this.serviceRegistrations;
            }
        }

        public ServiceRepositoryBootstrapperOptions AddService(Type serviceModuleType)
        {
            if (!serviceRegistrations.TryGetValue(serviceModuleType, out Func<IServiceProvider, IRepositoryContext> registryItem))
            {
                serviceRegistrations.TryAdd(serviceModuleType, null);
            }

            return new ServiceRepositoryBootstrapperOptions(serviceModuleType, serviceRegistrations);
        }

        public ServiceRepositoryBootstrapperOptions AddService<TService>() where TService : IServiceModule
        {
            return AddService(typeof(TService));
        }
    }
}
