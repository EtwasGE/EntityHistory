using System;
using EntityHistory.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityHistory.EntityFrameworkCore.Common.Configurations
{
    public class EntityPropertyChangeConfig<TEntityPropertyChange, TPrimaryKey> : IEntityTypeConfiguration<TEntityPropertyChange>
        where TEntityPropertyChange: EntityPropertyChange<TPrimaryKey> 
        where TPrimaryKey : struct, IEquatable<TPrimaryKey>
    {
        /// <summary>
        /// Maximum length of <see cref="EntityPropertyChange{TPrimaryKey}.PropertyName"/> property.
        /// Value: 96.
        /// </summary>
        public const int MaxPropertyNameLength = 96;

        /// <summary>
        /// Maximum length of <see cref="EntityPropertyChange{TPrimaryKey}.NewValue"/> and <see cref="EntityPropertyChange{TPrimaryKey}.OriginalValue"/> properties.
        /// Value: 512.
        /// </summary>
        public const int MaxValueLength = 512;

        /// <summary>
        /// Maximum length of <see cref="EntityPropertyChange{TPrimaryKey}.PropertyTypeFullName"/> property.
        /// Value: 192.
        /// </summary>
        public const int MaxPropertyTypeFullNameLength = 192;

        public void Configure(EntityTypeBuilder<TEntityPropertyChange> builder)
        {
            builder.ToTable("EntityHistoryEntityPropertyChanges");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.NewValue).HasMaxLength(MaxValueLength);
            builder.Property(p => p.OriginalValue).HasMaxLength(MaxValueLength);
            builder.Property(p => p.PropertyName).HasMaxLength(MaxPropertyNameLength);
            builder.Property(p => p.PropertyTypeFullName).HasMaxLength(MaxPropertyTypeFullNameLength);

            builder.HasIndex(e => e.EntityChangeId);
        }
    }
}
