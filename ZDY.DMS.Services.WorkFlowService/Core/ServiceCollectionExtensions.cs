using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.Services.WorkFlowService.Core.Interfaces;
using ZDY.DMS.Services.WorkFlowService.Core.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWorkflow(this IServiceCollection services)
        {
            services.AddTransient<ISignatureProvider, SignatureProvider>();

            services.AddTransient<IPersistenceProvider, PersistenceProvider>();

            services.AddTransient<IWorkFlowExecutor, WorkFlowExecutor>();

            return services;
        }
    }
}
