using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.AspNetCore.EntityMapper
{
    public interface IEntityMapperRegister : IDisposable
    {
        void CreateMap(Type sourceType, Type destinationType);

        void CreateMap<TSource, TDestination>();

        ConcurrentDictionary<Type, Type> GetMappers();
    }
}
