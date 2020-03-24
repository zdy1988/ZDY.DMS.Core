using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.Metronic.TagHelpers.Untils
{
    public class DataBindName
    {
        public DataBindName(string bindName, bool isAppend)
        {
            this.BindName = bindName;
            this.IsAppend = isAppend;
        }

        public string BindName { get; set; }
        public bool IsAppend { get; set; }
    }
}
