using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ZDY.DMS.AspNetCore.Module;
using ZDY.DMS.Repositories;

namespace ZDY.DMS.AspNetCore
{
    internal class ServiceBootstrapper
    {
        private readonly IServiceCollection services;

        private readonly ServiceBootstrapperOptions options = new ServiceBootstrapperOptions();

        public ServiceBootstrapper(IServiceCollection services)
        {
            this.services = services;
        }

        public void DoConfigure(Action<ServiceBootstrapperOptions> configure = null)
        {
            configure?.Invoke(options);

            if (options.ServiceModules.Count > 0)
            {
                // Register Modules
                foreach (var module in options.ServiceModules)
                {
                    services.AddSingleton(module.Key);
                }

                // Register RepositoryContext Owner Of The Module
                services.AddScoped(implementationFactory =>
                {
                    IRepositoryContext factory(Type serviceModuleType)
                    {
                        if (options.ServiceModules.TryGetValue(serviceModuleType, out Func<IServiceProvider, IRepositoryContext> registryRepositoryContextFactory))
                        {
                            return registryRepositoryContextFactory.Invoke(implementationFactory);
                        }

                        throw new InvalidOperationException($"Unable to resolve repository context for module '{serviceModuleType}'");
                    }

                    return (Func<Type, IRepositoryContext>)factory;
                });
            }
        }

        public void Initialize(IApplicationBuilder builder)
        {
            if (options.ServiceModules.Count > 0)
            {
                foreach (var item in options.ServiceModules)
                {
                    IServiceModule module = (IServiceModule)builder.ApplicationServices.GetService(item.Key);

                    module?.Initialize();
                }
            }
        }
    }
}
