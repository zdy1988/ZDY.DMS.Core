using System;

namespace ZDY.DMS.Models
{
    public class Log : BaseEntity
    {
        public string Message { get; set; }
        public int Type { get; set; }
    }
}
