﻿using EntityHistory.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityHistory.EntityFrameworkCore.Common.Configurations
{
    internal class EntityChangeConfig<TEntityChange> : IEntityTypeConfiguration<TEntityChange>
        where TEntityChange: EntityChange
    {
        public void Configure(EntityTypeBuilder<TEntityChange> builder)
        {
            builder.ToTable("EntityHistoryEntityChanges");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.EntityId).HasMaxLength(EntityChange.MaxEntityIdLength);
            builder.Property(p => p.EntityTypeFullName).HasMaxLength(EntityChange.MaxEntityTypeFullNameLength);

            builder.Ignore(p => p.EntityEntry);

            builder.HasMany(p => p.PropertyChanges)
                .WithOne()
                .HasForeignKey(p => p.EntityChangeId);

            builder.HasIndex(e => new { e.EntityChangeSetId });
            builder.HasIndex(e => new { e.EntityTypeFullName, e.EntityId });
        }
    }
}
