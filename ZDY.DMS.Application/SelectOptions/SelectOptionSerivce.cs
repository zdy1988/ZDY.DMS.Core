using System;
using System.Collections.Generic;
using System.Text;
using ZDY.DMS.Extensions.DependencyInjection.Autofac;

namespace ZDY.DMS.Application.SelectOptions
{
    public class SelectOptionSerivce : ISelectOptionSerivce
    {
        public List<KeyValuePair<string, string>> GetOptions<TKey>()
        {
            return ServiceLocator.GetService<ISelectOption<TKey>>().GenerateOptions();
        }
    }
}
