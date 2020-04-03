using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ZDY.DMS.AspNetCore.Dictionary;
using ZDY.DMS.AspNetCore.Service;
using ZDY.DMS.Caching;
using ZDY.DMS.Caching.InMemory;
using ZDY.DMS.Querying.DataTableGateway;
using ZDY.DMS.Querying.DataTableGateway.MySQL;
using ZDY.DMS.StringEncryption;

namespace ZDY.DMS.AspNetCore.Extensions.DependencyInjection
{
    public static class IntegrationExtensions
    {
        public static void AddDMS(this IServiceCollection services, Action<ServiceBootstrapperConfigurator> configure = null)
        {
            //注入http上下文和action上下文
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            //配置关闭默认ModelState验证
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            //注册缓存
            services.TryAddSingleton<ICacheManager, MemoryCacheManager>();

            //注入字典
            services.TryAddSingleton<IDictionaryRegister, DictionaryRegister>();
            services.TryAddSingleton<IDictionaryProvider, DictionaryProvider>();

            //加密方式
            //services.TryAddSingleton<IStringEncryption, MD5StringEncryption>();
            services.TryAddSingleton<IStringEncryption, NoStringEncryption>();

            //注入DataTableGateway
            services.TryAddSingleton<IDataTableGateway, MySqlDataTableGateway>();

            //注入AppSetting
            services.TryAddSingleton<IAppSettingProvider, AppSettingProvider>();

            //注入服务
            services.AddServiceAssembly();

            //注入AutoMapper
            services.AddAutoMapper();

            //注入数据权限控制
            //services.AddDataPermission();

            //注册 Service 模块 
            services.AddServiceBootstrapper(configure);
        }
    }

    public static class ServiceBootstrapperExtensions
    {
        public static void AddServiceBootstrapper(this IServiceCollection services, Action<ServiceBootstrapperConfigurator> configure = null)
        {
            var serviceBootstrapper = new ServiceBootstrapper(services);
            serviceBootstrapper.Configure(configure);
            services.AddSingleton<ServiceBootstrapper>(serviceBootstrapper);
        }
    }

    public static class ServiceRegisterExtensions
    {
        public static void AddServiceAssembly(this IServiceCollection services)
        {
            // 查找所有继承 IServiceBase 的类型
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IServiceBase)) && !t.IsAbstract && t.IsPublic))
                .ToArray();

            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces();
                foreach (var face in interfaces)
                {
                    if (!face.Equals(typeof(IServiceBase)))
                    {
                        services.TryAddSingleton(face, type);
                    }
                }
            }
        }
    }

    public static class EntityMapperServiceCollectionExtensions
    {
        public static void AddAutoMapper(this IServiceCollection services)
        {
            var profiles = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => typeof(Profile).IsAssignableFrom(t) && !t.IsAbstract && t.IsPublic))
                .ToList();

            services.AddSingleton<IMapper>(sp => new MapperConfiguration(config =>
            {
                foreach (var profile in profiles)
                {
                    config.AddProfile(profile);
                }

            }).CreateMapper());
        }
    }
}

namespace ZDY.DMS.AspNetCore.Extensions.Builder
{
    public static class IntegrationExtensions
    {
        public static void UseDMS(this IApplicationBuilder builder)
        {
            //使用错处处理
            builder.UseErrorHandle();

            //使用Autofac服务定位器
            builder.UseAutofacServiceLocator();

            //使用数据权限控制
            //builder.UseDataPermission();

            //初始化 Service 模块
            builder.UseServiceBootstrapper();
        }
    }

    public static class ServiceBootstrapperExtensions
    {
        public static void UseServiceBootstrapper(this IApplicationBuilder builder)
        {
            var serviceBootstrapper = builder.ApplicationServices.GetRequiredService<ServiceBootstrapper>();
            serviceBootstrapper?.Initialize(builder);
        }
    }

    public static class CookiesAuthorizationAppBuilderExtensions
    {
        public static IApplicationBuilder UseCookiesAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CookiesAuthenticationMiddleware>();
        }
    }

    public static class ExceptionHandleBuilderExtensions
    {
        public static IApplicationBuilder UseErrorHandle(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandleMiddleware>();
        }
    }
}
