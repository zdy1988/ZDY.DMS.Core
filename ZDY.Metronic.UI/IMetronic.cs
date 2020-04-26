using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.Metronic.UI
{
    public interface IMetronic
    {
        Dictionary<T, string> GetIconDictionary<T>();

        string GetIconContent(object icon);
    }
}
