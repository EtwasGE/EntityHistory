using System;
using System.Collections.Generic;
using EntityHistory.Core.Interfaces;

namespace EntityHistory.Core.Entities
{
    //public class EntityChange : EntityChange<long>, IEntity
    //{
    //}

    public class EntityChange<TPrimaryKey> : IEntity<TPrimaryKey> 
        where TPrimaryKey : struct, IEquatable<TPrimaryKey>
    {
        public virtual TPrimaryKey Id { get; set; }

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
        /// Unique key of the entity.
        /// </summary>
        public virtual string EntityUniqueKey { get; set; }

        /// <summary>
        /// Gets/sets change set id, used to group entity changes.
        /// </summary>
        public virtual TPrimaryKey EntityChangeSetId { get; set; }

        /// <summary>
        /// PropertyChanges.
        /// </summary>
        public virtual ICollection<EntityPropertyChange<TPrimaryKey>> PropertyChanges { get; set; }
        
        public virtual object EntityEntry { get; set; }
    }
}
