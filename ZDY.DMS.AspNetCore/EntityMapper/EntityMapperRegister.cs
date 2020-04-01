using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.AspNetCore.EntityMapper
{
    public class EntityMapperRegister : DisposableObject, IEntityMapperRegister
    {
        private readonly ConcurrentDictionary<Type, Type> mappers;

        public EntityMapperRegister()
        {
            mappers = new ConcurrentDictionary<Type, Type>();
        }

        public void CreateMap(Type sourceType, Type destinationType)
        {
            mappers.TryAdd(sourceType, destinationType);
        }

        public void CreateMap<TSource, TDestination>()
        {
            CreateMap(typeof(TSource), typeof(TDestination));
        }

        public ConcurrentDictionary<Type, Type> GetMappers()
        {
            return mappers;
        }
    }
}
