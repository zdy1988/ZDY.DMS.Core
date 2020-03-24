using Autofac;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ZDY.DMS.Extensions.DependencyInjection.Autofac
{
    public class ServiceLocator
    {
        private static ILifetimeScope _container;

        public static void SetContainer(ILifetimeScope container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            _container = container;
        }

        public static ILifetimeScope Container
        {
            get
            {
                return _container;
            }
        }

        public static TSerivce GetService<TSerivce>()
        {
            return Container.Resolve<TSerivce>();
        }

        public static TSerivce GetServiceByName<TSerivce>(string Name)
        {
            return Container.ResolveNamed<TSerivce>(Name);
        }

        public static TSerivce GetServiceByKey<TSerivce>(object Key)
        {
            return Container.ResolveKeyed<TSerivce>(Key);
        }
    }
}
