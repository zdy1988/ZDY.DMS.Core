﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Autofac;
using ZDY.DMS.Repositories;
using ZDY.DMS.Repositories.EntityFramework;
using ZDY.DMS.AspNetCore.Auth;
using ZDY.DMS.AspNetCore.Mvc.Filters;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.Services.AdminService;
using ZDY.DMS.Services.AuthService;
using ZDY.DMS.Services.MessageService;
using ZDY.DMS.Services.OrganizationService;
using ZDY.DMS.Services.PermissionService;
using ZDY.DMS.Services.UserService;
using ZDY.DMS.Services.WorkFlowService;
using ZDY.DMS.Web.Repositories.EntityFramework;
using ZDY.DMS.Querying.DataTableGateway;
using ZDY.DMS.Querying.DataTableGateway.MySQL;


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

            //注册认证
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

            //注册授权
            services.AddAuthorization(auth =>
            {
                auth.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser()
                    .Build();
            });

            //注册MVC
            services.AddMvc(options =>
            {
                options.Filters.Add(new AuthorizeFilter()); //全局授权
                options.Filters.Add(new ValidationFilter()); //数据验证
                options.Filters.Add(new ResponseFilter()); //响应重构
                options.RespectBrowserAcceptHeader = true;

            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.ContractResolver = new NullToEmptyStringResolver();
            });

            string conn = @"server=localhost;userid=root;pwd=1234;port=3306;database=test;sslmode=none;";

            services.AddDbContext<DMSDbContext>(options => options.UseMySql(conn, b => b.MigrationsAssembly("ZDY.DMS.Web")), ServiceLifetime.Transient, ServiceLifetime.Transient);

            //仓储
            services.AddTransient<IRepositoryContext>(sp => new EntityFrameworkRepositoryContext(sp.GetService<DMSDbContext>()));

            services.AddTransient<IDataTableGateway>(sp => new MySqlDataTableGateway(conn));

            //工作流
            services.AddWorkflow();

            //DMS
            services.AddDMS(config => {
                config.AddService<AdminServiceModule>().WithRepository(sp => sp.GetService<IRepositoryContext>());
                config.AddService<AuthServiceModule>().WithRepository(sp => sp.GetService<IRepositoryContext>());
                config.AddService<MessageServiceModule>().WithRepository(sp => sp.GetService<IRepositoryContext>());
                config.AddService<OrganizationServiceModule>().WithRepository(sp => sp.GetService<IRepositoryContext>());
                config.AddService<PermissionServiceModule>().WithRepository(sp => sp.GetService<IRepositoryContext>());
                config.AddService<UserServiceModule>().WithRepository(sp => sp.GetService<IRepositoryContext>());
                config.AddService<WorkFlowServiceModule>().WithRepository(sp => sp.GetService<IRepositoryContext>()).WithDataTableGateway(sp => sp.GetService<IDataTableGateway>());

                config.UseEventBus();
            });

            //UI
            services.AddMetronicUI(setting => {

                setting.ModalDismissText = "关闭";

                setting.TextBoxPlaceholderFormat = "请输入{0}...";

                setting.SelectBoxPlaceholderFormat = "请选择{0}...";

                setting.IsFormBoxHelpTextDestroyed = true;

            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<SelectOptionsFactory>().SingleInstance();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseTokenProvider();

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
