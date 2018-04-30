using System;
using System.Collections.Generic;
using EntityHistory.Abstractions;

namespace EntityHistory.Core.Entities
{
    public class EntityChange<TKey> : IEntity<TKey> 
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Maximum length of <see cref="EntityChange{TKey}.EntityId"/> property.
        /// Value: 48.
        /// </summary>
        public const int MaxEntityIdLength = 48;

        /// <summary>
        /// Maximum length of <see cref="EntityChange{TKey}.EntityTypeFullName"/> property.
        /// Value: 192.
        /// </summary>
        public const int MaxEntityTypeFullNameLength = 192;

        /// <summary>
        /// Maximum length of <see cref="EntityChange{TKey}.EntityUniqueKey"/> property.
        /// Value: 512.
        /// </summary>
        public const int MaxEntityUniqueKeyLength = 512;

        public virtual TKey Id { get; set; }

        /// <summary>
        /// ChangeType.
        /// </summary>
        public virtual EntityChangeType ChangeType { get; set; }

        /// <summary>
        /// Gets/sets primary key of the entity.
        /// </summary>
        public virtual string EntityId { get; set; }

        /// <summary>
        /// FullName of the entity type.
        /// </summary>
        public virtual string EntityTypeFullName { get; set; }
        
        /// <summary>
        /// Gets/sets change set id, used to group entity changes.
        /// </summary>
        public virtual TKey EntityChangeSetId { get; set; }

        /// <summary>
        /// PropertyChanges.
        /// </summary>
        public virtual ICollection<EntityPropertyChange<TKey>> PropertyChanges { get; set; }
        
        public virtual object EntityEntry { get; set; }
    }
}
