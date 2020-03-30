using AutoMapper;
using ZDY.DMS.Services.Common.AutoMapper;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AutoMapperServiceCollectionExtensions
    {
        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddSingleton<IMapper>(sp => (new MapperConfiguration(cfg =>
            {
                //cfg.AddProfile(new AutoMapProfileConfiguration());
                cfg.AddProfile(new AutoMapperProfileConfiguration());
            }).CreateMapper()));
        }
    }
}
