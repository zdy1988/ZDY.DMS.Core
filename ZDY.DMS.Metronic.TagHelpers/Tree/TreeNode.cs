using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.Metronic.TagHelpers.Tree
{
    public class TreeNode
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid ParentId { get; set; }

        public object Data { get; set; }

        public int Order { get; set; }
    }
}
