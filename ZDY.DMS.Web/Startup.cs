using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Autofac;
using ZDY.DMS.Repositories;
using ZDY.DMS.Repositories.EntityFramework;
using ZDY.DMS.AspNetCore.Mvc.Filters;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.Services.OrganizationService.Implementation;
using ZDY.DMS.Services.AdminService.Implementation;
using ZDY.DMS.Services.WorkFlowService.Implementation;
using ZDY.DMS.AspNetCore;
using ZDY.DMS.Services.OrganizationService.ServiceContracts;
using ZDY.DMS.Services.AdminService.ServiceContracts;
using ZDY.DMS.Services.WorkFlowService.ServiceContracts;
using ZDY.DMS.Services.PermissionService.ServiceContracts;
using ZDY.DMS.Services.PermissionService.Implementation;
using ZDY.DMS.Services.Common.Implementation;
using ZDY.DMS.Services.Common.ServiceContracts;
using ZDY.DMS.Web.Repositories.EntityFramework;
using ZDY.DMS.Services.AdminService;
using ZDY.DMS.AspNetCore.Auth;
using ZDY.DMS.Services.AuthService;
using ZDY.DMS.Services.MessageService;
using ZDY.DMS.Services.OrganizationService;
using ZDY.DMS.Services.PermissionService;
using ZDY.DMS.Services.UserService;
using ZDY.DMS.Services.WorkFlowService;

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

            //注册验证
            services.AddAuthorization(auth =>
            {
                auth.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser()
                    .Build();
            });

            services.AddAuthentication(options => options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = SecurityTokenOptions.Key,
                        ValidAudience = SecurityTokenOptions.Audience,
                        ValidIssuer = SecurityTokenOptions.Issuer,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(0)
                    });

            //注册MVC
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


            services.AddDbContext<DMSDbContext>(options => options.UseMySql(@"server=localhost;userid=root;pwd=1234;port=3306;database=test;sslmode=none;", b => b.MigrationsAssembly("ZDY.DMS.Web")));

            //仓储
            services.AddScoped<IRepositoryContext>(sp => new EntityFrameworkRepositoryContext(sp.GetService<DMSDbContext>()));

            //DMS
            services.AddDMS(options => {
                options.AddService<AdminServiceModule>().WithRepository(sp => sp.GetService<IRepositoryContext>());
                options.AddService<AuthServiceModule>().WithRepository(sp => sp.GetService<IRepositoryContext>());
                options.AddService<MessageServiceModule>().WithRepository(sp => sp.GetService<IRepositoryContext>());
                options.AddService<OrganizationServiceModule>().WithRepository(sp => sp.GetService<IRepositoryContext>());
                options.AddService<PermissionServiceModule>().WithRepository(sp => sp.GetService<IRepositoryContext>());
                options.AddService<UserServiceModule>().WithRepository(sp => sp.GetService<IRepositoryContext>());
                options.AddService<WorkFlowServiceModule>().WithRepository(sp => sp.GetService<IRepositoryContext>());
            });

            services.AddAutoMapper();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //服务
            builder.RegisterType<AppSettingService>().As<IAppSettingService>();
            builder.RegisterType<DictionaryService>().As<IDictionaryService>();
            builder.RegisterType<StaticFileService>().As<IStaticFileService>();

            builder.RegisterType<SelectOptionsFactory>().SingleInstance();

            builder.RegisterType<DepartmentService>().As<IDepartmentService>().SingleInstance();
            builder.RegisterType<PageService>().As<IPageService>().SingleInstance();
            builder.RegisterType<PagePermissionService>().As<IPagePermissionService>().SingleInstance();

            builder.RegisterType<WorkFlowService>().As<IWorkFlowService>().SingleInstance();
            builder.RegisterType<WorkFlowFormService>().As<IWorkFlowFormService>().SingleInstance();
            builder.RegisterType<WorkFlowHostService>().As<IWorkFlowHostService>().SingleInstance();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCookiesAuthentication();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseDMS();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapRazorPages();
            });
        }
    }
}
