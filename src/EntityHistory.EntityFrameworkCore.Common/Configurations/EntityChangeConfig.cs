using System;
using EntityHistory.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityHistory.EntityFrameworkCore.Common.Configurations
{
    public class EntityChangeConfig<TEntityChange, TPrimaryKey> : IEntityTypeConfiguration<TEntityChange>
        where TEntityChange: EntityChange<TPrimaryKey> 
        where TPrimaryKey : struct, IEquatable<TPrimaryKey>
    {
        /// <summary>
        /// Maximum length of <see cref="EntityChange{TPrimaryKey}.EntityId"/> property.
        /// Value: 48.
        /// </summary>
        public const int MaxEntityIdLength = 48;

        /// <summary>
        /// Maximum length of <see cref="EntityChange{TPrimaryKey}.EntityTypeFullName"/> property.
        /// Value: 192.
        /// </summary>
        public const int MaxEntityTypeFullNameLength = 192;

        /// <summary>
        /// Maximum length of <see cref="EntityChange{TPrimaryKey}.EntityUniqueKey"/> property.
        /// Value: 512.
        /// </summary>
        public const int MaxEntityUniqueKeyLength = 512;

        public void Configure(EntityTypeBuilder<TEntityChange> builder)
        {
            builder.ToTable("EntityHistoryEntityChanges");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.EntityId).HasMaxLength(MaxEntityIdLength);
            builder.Property(p => p.EntityTypeFullName).HasMaxLength(MaxEntityTypeFullNameLength);
            builder.Property(p => p.EntityUniqueKey).HasMaxLength(MaxEntityUniqueKeyLength);

            builder.Ignore(p => p.EntityEntry);

            builder.HasMany(p => p.PropertyChanges)
                .WithOne()
                .HasForeignKey(p => p.EntityChangeId);

            builder.HasIndex(e => new { e.EntityChangeSetId });
            builder.HasIndex(e => new { e.EntityTypeFullName, e.EntityId });
            builder.HasIndex(e => new { e.EntityUniqueKey });
        }
    }
}
