using System;

namespace ZDY.DMS.Models
{
    public class BaseEntity : IEntity<Guid>
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime TimeStamp { get; set; } = DateTime.Now;

        public int Version { get; set; } = 0;
    }
}
