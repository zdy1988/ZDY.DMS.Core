using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.Application.SelectOptions
{
    public interface ISelectOption<TKey>
    {
        List<KeyValuePair<string, string>> GenerateOptions();
    }
}
