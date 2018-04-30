using System;
using EntityHistory.Abstractions;

namespace EntityHistory.Core.Entities
{
    public class EntityPropertyChange<TKey> : IEntity<TKey> 
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Maximum length of <see cref="EntityPropertyChange{TKey}.PropertyName"/> property.
        /// Value: 96.
        /// </summary>
        public const int MaxPropertyNameLength = 96;

        /// <summary>
        /// Maximum length of <see cref="EntityPropertyChange{TKey}.NewValue"/> and <see cref="EntityPropertyChange{TKey}.OriginalValue"/> properties.
        /// Value: 512.
        /// </summary>
        public const int MaxValueLength = 512;

        /// <summary>
        /// Maximum length of <see cref="EntityPropertyChange{TKey}.PropertyTypeFullName"/> property.
        /// Value: 192.
        /// </summary>
        public const int MaxPropertyTypeFullNameLength = 192;

        public virtual TKey Id { get; set; }

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
        public virtual TKey EntityChangeId { get; set; }
    }
}
