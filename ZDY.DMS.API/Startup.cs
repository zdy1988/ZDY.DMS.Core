using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Autofac;
using ZDY.DMS.Caching.InMemory;
using ZDY.DMS.Caching;
using ZDY.DMS.Repositories;
using ZDY.DMS.Repositories.EntityFramework;
using Microsoft.EntityFrameworkCore;
using ZDY.DMS.AspNetCore.Mvc;
using ZDY.DMS.API.Swagger;
using ZDY.DMS.StringEncryption;
using ZDY.DMS.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ZDY.DMS.Querying.DataTableGateway;
using ZDY.DMS.Querying.DataTableGateway.MySQL;
using ZDY.DMS.Services.Shared.ServiceContracts;
using ZDY.DMS.Services.AdminService.Implementation;
using ZDY.DMS.API.Repositories.EntityFramework;
using ZDY.DMS.AspNetCore;
using ZDY.DMS.Services.AdminService.ServiceContracts;
using ZDY.DMS.Services.AdminService;
using ZDY.DMS.Services.AuthService;
using ZDY.DMS.Services.MessageService;
using ZDY.DMS.Services.OrganizationService;
using ZDY.DMS.Services.PermissionService;
using ZDY.DMS.Services.UserService;
using ZDY.DMS.Services.WorkFlowService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ZDY.DMS.AspNetCore.Auth;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace ZDY.DMS.API
{
    public class Startup
    {
        private readonly string projectName = "ZDY.DMS.Web API";

        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddControllersAsServices();

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

            services.AddMvc(options =>
            {
                //options.Filters.Add(new AuthorizeFilter()); //全局权限
                options.Filters.Add(new ValidationFilter()); //数据验证
                options.Filters.Add(new ResponseFilter()); //响应重构
                options.RespectBrowserAcceptHeader = true;

            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.ContractResolver = new NullToEmptyStringResolver();
            });

            string conn = @"server=localhost;userid=root;pwd=1234;port=3306;database=test;sslmode=none;";

            services.AddDbContext<ApiDbContext>(options => options.UseMySql(conn, b => b.MigrationsAssembly("ZDY.DMS.Web")), ServiceLifetime.Transient, ServiceLifetime.Transient);

            //仓储
            services.AddTransient<IRepositoryContext>(sp => new EntityFrameworkRepositoryContext(sp.GetService<ApiDbContext>()));

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

            services.AddSwaggerGen(c =>
            {
                typeof(ApiVersions).GetEnumNames().ToList().ForEach(version =>
                {

                    c.SwaggerDoc(version, new OpenApiInfo
                    {
                        Version = version,
                        Title = $"{projectName} 接口文档",
                        Description = $"{projectName} HTTP API " + version,
                        TermsOfService = new Uri("http://zdyla.com"),
                        Contact = new OpenApiContact
                        {
                            Name = "Zdy",
                            Email = "virus_zhh@126.com",
                            Url = new Uri("http://zdyla.com"),
                        },
                        License = new OpenApiLicense
                        {
                            Name = "Apache 2.0",
                            Url = new Uri("http://www.apache.org/licenses/LICENSE-2.0.html"),
                        }
                    });

                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.OperationFilter<SwaggerOperationFilter>();
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
         
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                typeof(ApiVersions).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version =>
                {
                    c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{projectName} {version}");
                });
            });

            app.UseRouting();

            app.UseTokenProvider();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseDMS();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
