using System;

namespace ZDY.DMS.Services.Shared.Models
{
    public class BaseEntity : IEntity<Guid>
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime TimeStamp { get; set; } = DateTime.Now;
    }
}
