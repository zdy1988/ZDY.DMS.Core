using Autofac;
using Microsoft.EntityFrameworkCore;
using ZDY.DMS.Repositories;
using ZDY.DMS.Domain.Repositories.EntityFramework;

namespace ZDY.DMS.Application.Autofac
{
    public class DbContextModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<JxcDbContext>()
            //    .Named<DbContext>(ConfigurationManager.DbContextName);
            //builder.RegisterType<EntityFrameworkRepositoryContext>()
            //    .As<IRepositoryContext>()
            //    .WithParameter(new TypedParameter(typeof(string), ConfigurationManager.DbContextName))
            //    .InstancePerLifetimeScope();
        }
    }
}
