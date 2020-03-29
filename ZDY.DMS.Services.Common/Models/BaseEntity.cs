using System;

namespace ZDY.DMS.Services.Common.Models
{
    public class BaseEntity : IEntity<Guid>
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime TimeStamp { get; set; } = DateTime.Now;
    }
}
