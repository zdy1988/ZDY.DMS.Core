using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ZDY.DMS.AspNetCore.Dictionary;
using ZDY.DMS.Caching;
using ZDY.DMS.Caching.InMemory;
using ZDY.DMS.KeyGeneration;
using ZDY.DMS.Querying.DataTableGateway;
using ZDY.DMS.Querying.DataTableGateway.MySQL;
using ZDY.DMS.StringEncryption;

namespace ZDY.DMS.AspNetCore
{
    public static class IntegrationExtensions
    {
        public static void AddDMS(this IServiceCollection services, Action<ServiceBootstrapperOptions> configure = null)
        {
            //注入http上下文和action上下文
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            //注入数据权限控制
            //services.AddDataPermission();

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

            //注册 Service 模块 
            var serviceBootstrapper = new ServiceBootstrapper(services);
            serviceBootstrapper.DoConfigure(configure);
            services.AddSingleton<ServiceBootstrapper>(serviceBootstrapper);
        }

        public static IApplicationBuilder UseDMS(this IApplicationBuilder builder)
        {
            //使用错处处理
            builder.UseErrorHandle();

            //使用Autofac服务定位器
            builder.UseAutofacServiceLocator();

            //使用数据权限控制
            //builder.UseDataPermission();

            //初始化 Service 模块
            var serviceBootstrapper = builder.ApplicationServices.GetRequiredService<ServiceBootstrapper>();
            serviceBootstrapper?.Initialize(builder);
            return builder;
        }
    }
}
