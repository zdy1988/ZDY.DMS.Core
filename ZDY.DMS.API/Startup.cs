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
using ZDY.DMS.Services.Common.ServiceContracts;
using ZDY.DMS.Services.AdminService.Implementation;
using ZDY.DMS.API.Repositories.EntityFramework;
using ZDY.DMS.AspNetCore;
using ZDY.DMS.Services.AdminService.ServiceContracts;

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

            services.AddDbContext<ApiDbContext>(options => options.UseMySql(@"server=localhost;userid=root;pwd=1234;port=3306;database=test;sslmode=none;"));

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ApiValidationFilter));
                //options.Filters.Add(typeof(ApiDataPermissionFilter));
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
            //Http
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();

            //缓存
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().SingleInstance();

            //Ado
            builder.RegisterType<MySqlDataTableGateway>().As<IDataTableGateway>().SingleInstance();

            //仓储
            builder.Register<IRepositoryContext>(ctx => new EntityFrameworkRepositoryContext(ctx.Resolve<ApiDbContext>())).InstancePerLifetimeScope();

            //加密
            builder.RegisterType<MD5StringEncryption>().As<IStringEncryption>();

            //服务
            builder.RegisterType<AppSettingProvider>().As<IAppSettingProvider>();
            builder.RegisterType<DictionaryService>().As<IDictionaryService>();
            builder.RegisterType<FileService>().As<IFileService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseErrorHandle();

            app.UseAutofacServiceLocator();

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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
