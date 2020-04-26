using System;
using System.Collections.Generic;
using System.Text;
using ZDY.Metronic.UI.Icons;

namespace ZDY.Metronic.UI
{
    public class Metronic : IMetronic
    {
        public string GetIconContent(object icon)
        {
            return icon.ToIconContent();
        }

        public Dictionary<T, string> GetIconDictionary<T>()
        {
            if (typeof(T) == typeof(FlatIcon))
            {
                return Flat.All as Dictionary<T, string>;
            }
            else if (typeof(T) == typeof(Flat2Icon))
            {
                return Flat2.All as Dictionary<T, string>;
            }
            else if (typeof(T) == typeof(FontawesomeIcon))
            {
                return Fontawesome.All as Dictionary<T, string>;
            }
            else if (typeof(T) == typeof(LineawesomeIcon))
            {
                return Lineawesome.All as Dictionary<T, string>;
            }
            else if (typeof(T) == typeof(SocIcon))
            {
                return Soc.All as Dictionary<T, string>;
            }
            else if (typeof(T) == typeof(SvgIcon))
            {
                return Svg.All as Dictionary<T, string>;
            }

            throw new NotImplementedException();
        }
    }
}
