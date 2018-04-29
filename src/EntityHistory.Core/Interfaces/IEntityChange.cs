using System;

namespace EntityHistory.Core.Interfaces
{
    public interface IEntityChange
    {
        /// <summary>
        /// ChangeTime.
        /// </summary>
        DateTime ChangeTime { get; set; }

        /// <summary>
        /// ChangeType.
        /// </summary>
        EntityChangeType ChangeType { get; set; }
        
        /// <summary>
        /// Gets/sets primary key of the entity.
        /// </summary>
        string EntityId { get; set; }

        /// <summary>
        /// FullName of the entity type.
        /// </summary>
        string EntityTypeFullName { get; set; }

        /// <summary>
        /// Unique key of the entity.
        /// </summary>
        string EntityUniqueKey { get; set; }
    }
}
