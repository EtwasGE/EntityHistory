using System;
using System.Collections.Generic;

namespace EntityHistory.Core.Entities
{
    public class EntityChange
    {
        public EntityChange()
        {
            PropertyChanges = new HashSet<EntityPropertyChange>();
        }

        /// <summary>
        /// Maximum length of <see cref="EntityId"/> property.
        /// Value: 48.
        /// </summary>
        public const int MaxEntityIdLength = 48;

        /// <summary>
        /// Maximum length of <see cref="EntityTypeFullName"/> property.
        /// Value: 192.
        /// </summary>
        public const int MaxEntityTypeFullNameLength = 192;

        /// <summary>
        /// Primary Key
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ChangeType.
        /// </summary>
        public EntityChangeType ChangeType { get; set; }

        /// <summary>
        /// Gets/sets primary key of the entity.
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// FullName of the entity type.
        /// </summary>
        public string EntityTypeFullName { get; set; }

        /// <summary>
        /// Gets/sets change set id, used to group entity changes.
        /// </summary>
        public Guid EntityChangeSetId { get; set; }

        /// <summary>
        /// PropertyChanges.
        /// </summary>
        public virtual ICollection<EntityPropertyChange> PropertyChanges { get; set; }

        public object EntityEntry { get; set; }
    }
}
