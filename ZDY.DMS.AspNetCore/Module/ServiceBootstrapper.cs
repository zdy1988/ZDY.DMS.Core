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

            if (options.Services.Count > 0)
            {
                // Register Services
                foreach (var service in options.Services)
                {
                    services.AddSingleton(service.Key);
                }


                // Register RepositoryContext Owner Of The Service
                services.AddScoped(implementationFactory =>
                {
                    Func<Type, IRepositoryContext> func = serviceType =>
                    {
                        if (options.Services.TryGetValue(serviceType, out Func<IServiceProvider, IRepositoryContext> registryRepositoryContextFactory))
                        {
                            return registryRepositoryContextFactory.Invoke(implementationFactory);
                        }

                        throw new InvalidOperationException($"Unable to resolve service for type '{typeof(IRepositoryContext)}'");
                    };

                    return func;
                });
            }
        }

        public void Initialize(IApplicationBuilder builder)
        {
            if (options.Services.Count > 0)
            {
                foreach (var item in options.Services)
                {
                    IServiceModule service = (IServiceModule)builder.ApplicationServices.GetService(item.Key);

                    service.Initialize();
                }
            }
        }
    }
}
