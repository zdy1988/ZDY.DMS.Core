using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Autofac;
using Zdy.Events;
using ZDY.DMS.Caching.InMemory;
using ZDY.DMS.Caching;
using ZDY.DMS.Querying.AdoNet;
using ZDY.DMS.Querying.MySql;
using ZDY.DMS.Repositories;
using ZDY.DMS.Repositories.EntityFramework;
using ZDY.DMS.Domain.Repositories.EntityFramework;
using ZDY.DMS.Domain.EventHandlers;
using ZDY.DMS.Domain.Events;
using ZDY.DMS.AspNetCore.Mvc.Filters;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.Application.Implementation;
using ZDY.DMS.ServiceContracts;
using ZDY.DMS.StringEncryption;
using ZDY.DMS.Services.OrganizationService.Implementation;
using ZDY.DMS.Services.AdminService.Implementation;
using ZDY.DMS.Services.WorkFlowService.Implementation;
using ZDY.DMS.AspNetCore.Auth;
using ZDY.DMS.Services.OrganizationService.ServiceContracts;
using ZDY.DMS.Services.AdminService.ServiceContracts;
using ZDY.DMS.Services.WorkFlowService.ServiceContracts;

namespace ZDY.DMS.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddControllersAsServices();

            services.AddRazorPages();

            //services.AddAuthorization(auth => auth.DefaultPolicy = new AuthorizationPolicyBuilder().AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​).RequireAuthenticatedUser().Build());

            //services.AddAuthentication(options => options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme)
            //        .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            IssuerSigningKey = SecurityTokenOptions.Key,
            //            ValidAudience = SecurityTokenOptions.Audience,
            //            ValidIssuer = SecurityTokenOptions.Issuer,
            //            ValidateIssuerSigningKey = true,
            //            ValidateLifetime = true,
            //            ClockSkew = TimeSpan.FromMinutes(0)
            //        });

            //services.AddDataPermission();

            services.AddDbContext<JxcDbContext>(options => options.UseMySql(@"server=localhost;userid=root;pwd=1234;port=3306;database=test;sslmode=none;", b => b.MigrationsAssembly("ZDY.DMS.Web")));

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ApiValidationFilter));
                options.Filters.Add(typeof(ApiResponseFilter));
                options.RespectBrowserAcceptHeader = true;

            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.ContractResolver = new NullToEmptyStringResolver();
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddAutoMapper();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //Http
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();

            //缓存
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().SingleInstance();

            //Ado
            builder.RegisterType<MySqlDbCommand>().As<IAdoNetDbCommand>().SingleInstance();

            //仓储
            builder.Register<IRepositoryContext>(ctx => new EntityFrameworkRepositoryContext(ctx.Resolve<JxcDbContext>())).InstancePerLifetimeScope();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterType<EventBus>().As<IEventBus>();

            //加密
            //builder.RegisterType<MD5StringEncryption>().As<IStringEncryption>();
            builder.RegisterType<NoStringEncryption>().As<IStringEncryption>();

            //事件
            builder.RegisterType<LoggingEventHandler>().As<IEventHandler<LoggingEvent>>();
            builder.RegisterType<SendEmailEventHandler>().As<IEventHandler<SendEmailEvent>>();
            builder.RegisterType<SendMessageEventHandler>().As<IEventHandler<SendMessageEvent>>();

            //服务
            builder.RegisterType<AppSettingService>().As<IAppSettingService>();
            builder.RegisterType<DictionaryService>().As<IDictionaryService>();
            builder.RegisterType<StaticFileService>().As<IStaticFileService>();

            builder.RegisterType<SelectOptionsFactory>().SingleInstance();

            builder.RegisterType<DepartmentService>().As<IDepartmentService>().SingleInstance();
            builder.RegisterType<PageService>().As<IPageService>().SingleInstance();

            builder.RegisterType<WorkFlowService>().As<IWorkFlowService>().SingleInstance();
            builder.RegisterType<WorkFlowFormService>().As<IWorkFlowFormService>().SingleInstance();
            builder.RegisterType<WorkFlowWorkingService>().As<IWorkFlowWorkingService>().SingleInstance();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseErrorHandle();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseServiceLocator();

            //app.UseCookiesAuthentication();

            //app.UseAuthentication();

            //app.UseAuthorization();

            //app.UseDataPermission();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapRazorPages();
            });
        }
    }
}
