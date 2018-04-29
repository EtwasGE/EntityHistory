using System;

namespace EntityHistory.Core.Interfaces
{
    public interface IEntity : IEntity<long>
    {
    }

    public interface IEntity<TPrimaryKey>
        where TPrimaryKey : struct, IEquatable<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
    }
}
