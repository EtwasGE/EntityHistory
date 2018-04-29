using System;
using EntityHistory.Core.Interfaces;

namespace EntityHistory.Core.Entities
{
    //public class EntityPropertyChange : EntityPropertyChange<long>, IEntity
    //{
    //}

    public class EntityPropertyChange<TPrimaryKey> : IEntity<TPrimaryKey> 
        where TPrimaryKey : struct, IEquatable<TPrimaryKey>
    {
        public virtual TPrimaryKey Id { get; set; }

        /// <summary>
        /// NewValue.
        /// </summary>
        public virtual string NewValue { get; set; }

        /// <summary>
        /// OriginalValue.
        /// </summary>
        public virtual string OriginalValue { get; set; }

        /// <summary>
        /// PropertyName.
        /// </summary>
        public virtual string PropertyName { get; set; }

        /// <summary>
        /// Type of the JSON serialized <see cref="NewValue"/> and <see cref="OriginalValue"/>.
        /// It's the FullName of the type.
        /// </summary>
        public virtual string PropertyTypeFullName { get; set; }

        /// <summary>
        /// EntityChangeId.
        /// </summary>
        public virtual TPrimaryKey EntityChangeId { get; set; }
    }
}
