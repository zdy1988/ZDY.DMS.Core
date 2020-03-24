using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZDY.DMS.Models
{
    [Table("Dictionary_Value")]
    public class DictionaryValue : BaseEntity
    {
        public string DictionaryKey { get; set; }
        public string ParentValue { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Note { get; set; }
        public int Order { get; set; } = 0;
    }
}
