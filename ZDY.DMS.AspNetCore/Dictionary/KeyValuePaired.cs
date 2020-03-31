using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.AspNetCore.Dictionary
{
    public class KeyValuePaired
    {
        public KeyValuePaired(string value, string name, string parent)
        {
            this.Value = value;
            this.Name = name;
            this.Parent = parent;
        }

        public string Value { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }
    }
}
