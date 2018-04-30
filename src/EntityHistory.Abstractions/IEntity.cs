using System;

namespace EntityHistory.Abstractions
{
    public interface IEntity : IEntity<long>
    {
    }

    public interface IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        TKey Id { get; set; }
    }
}
