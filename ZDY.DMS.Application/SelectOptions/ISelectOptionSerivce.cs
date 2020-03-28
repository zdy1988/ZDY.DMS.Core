using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.Application.SelectOptions
{
    public interface ISelectOptionSerivce
    {
        List<KeyValuePair<string, string>> GetOptions<TKey>();
    }
}
