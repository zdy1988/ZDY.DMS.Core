using Autofac;
using Autofac.Extensions.DependencyInjection;
using ZDY.DMS;

namespace Microsoft.AspNetCore.Builder
{
    public static class ServiceLocatorBuilderExtensions
    {
        public static IApplicationBuilder UseAutofacServiceLocator(this IApplicationBuilder builder)
        {
            var container = builder.ApplicationServices.GetAutofacRoot();

            ServiceLocator.SetContainer(container);

            return builder;
        }
    }
}
