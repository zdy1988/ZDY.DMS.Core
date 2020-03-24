using Autofac;
using Autofac.Extensions.DependencyInjection;
using ZDY.DMS.Extensions.DependencyInjection.Autofac;

namespace Microsoft.AspNetCore.Builder
{
    public static class ServiceLocatorBuilderExtensions
    {
        public static IApplicationBuilder UseServiceLocator(this IApplicationBuilder builder)
        {
            var container = builder.ApplicationServices.GetAutofacRoot();

            ServiceLocator.SetContainer(container);

            return builder;
        }
    }
}
