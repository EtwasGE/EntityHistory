using System;
using EntityHistory.Abstractions;

namespace EntityHistory.Core.Entities
{
    public class EntityPropertyChange : IEntity<Guid> 
    {
        /// <summary>
        /// Maximum length of <see cref="PropertyName"/> property.
        /// Value: 96.
        /// </summary>
        public const int MaxPropertyNameLength = 96;

        /// <summary>
        /// Maximum length of <see cref="NewValue"/> and <see cref="OriginalValue"/> properties.
        /// Value: 512.
        /// </summary>
        public const int MaxValueLength = 512;

        /// <summary>
        /// Maximum length of <see cref="PropertyTypeFullName"/> property.
        /// Value: 192.
        /// </summary>
        public const int MaxPropertyTypeFullNameLength = 192;

        /// <summary>
        /// Primary Key
        /// </summary>
        public virtual Guid Id { get; set; }

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
        public virtual Guid EntityChangeId { get; set; }
    }
}
