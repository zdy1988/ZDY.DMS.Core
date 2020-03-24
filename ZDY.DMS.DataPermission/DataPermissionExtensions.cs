using System;
using ZDY.DMS.DataPermission;


namespace Microsoft.Extensions.DependencyInjection
{
    public static class DataPermissionExtensions
    {
        public static void AddDataPermission(this IServiceCollection services)
        {
            services.AddSingleton<IDataPermissionContext, DataPermissionContext>();
        }
    }
}

namespace Microsoft.AspNetCore.Builder
{
    public static class DataPermissionBuilderExtensions
    {
        public static IApplicationBuilder UseDataPermission(this IApplicationBuilder app)
        {
            return app.UseMiddleware<DataPermissionMiddleware>();
        }
    }
}