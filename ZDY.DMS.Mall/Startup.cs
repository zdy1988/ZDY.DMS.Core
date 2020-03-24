using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ZDY.DMS;
using ZDY.DMS.Application.Autofac;
using ZDY.DMS.Application.AutoMapper;
using ZDY.DMS.Domain.Repositories.EntityFramework;

namespace Zdy.Mall
{
    public class Startup
    {
        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();
           
            //context
            services.AddDbContext<JxcDbContext>(options => options.UseMySql(_configuration.GetConnectionString("MySql"), b => b.MigrationsAssembly("ZDY.DMS.API")));


            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            //automapper
            services.AddSingleton<IMapper>(sp => (new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapProfileConfiguration());
                cfg.AddProfile(new AutoMapperProfileConfiguration());
            }).CreateMapper()));

            //autofac
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<DbContextModule>();
            containerBuilder.RegisterModule<AutofacModule>();
            containerBuilder.RegisterModule<RepositoryModule>();
            containerBuilder.RegisterModule<ServiceModule>();
            containerBuilder.Populate(services);
            var autofacContainer = ServiceLocator.BuildContainer(containerBuilder);
            return new AutofacServiceProvider(autofacContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(_configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
