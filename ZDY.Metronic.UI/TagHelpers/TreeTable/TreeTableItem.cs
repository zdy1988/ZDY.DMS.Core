using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.Metronic.UI
{
    public class TreeTableItem
    {
        public Guid Id { get; set; }

        public Guid ParentId { get; set; }

        public object Data { get; set; }

        public int Order { get; set; }
    }
}
