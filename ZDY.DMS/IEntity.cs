using System;

namespace ZDY.DMS
{
    public interface IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        TKey Id { get; set; }

        DateTime TimeStamp { get; set; }
    }
}
