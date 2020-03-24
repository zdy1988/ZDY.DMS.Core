using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.Extensions.DependencyInjection.Autofac;

namespace ZDY.DMS.Application.SelectOptions
{
    public interface ISelectOptionSerivce
    {
        List<KeyValuePair<string, string>> GetOptions<TKey>();
    }
}
