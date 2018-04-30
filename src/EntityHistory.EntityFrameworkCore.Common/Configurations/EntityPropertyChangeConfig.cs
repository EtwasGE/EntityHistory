using System;
using EntityHistory.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityHistory.EntityFrameworkCore.Common.Configurations
{
    public class EntityPropertyChangeConfig<TEntityPropertyChange, TKey> : IEntityTypeConfiguration<TEntityPropertyChange>
        where TEntityPropertyChange: EntityPropertyChange<TKey> 
        where TKey : IEquatable<TKey>
    {
        public void Configure(EntityTypeBuilder<TEntityPropertyChange> builder)
        {
            builder.ToTable("EntityHistoryEntityPropertyChanges");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.NewValue).HasMaxLength(EntityPropertyChange<TKey>.MaxValueLength);
            builder.Property(p => p.OriginalValue).HasMaxLength(EntityPropertyChange<TKey>.MaxValueLength);
            builder.Property(p => p.PropertyName).HasMaxLength(EntityPropertyChange<TKey>.MaxPropertyNameLength);
            builder.Property(p => p.PropertyTypeFullName).HasMaxLength(EntityPropertyChange<TKey>.MaxPropertyTypeFullNameLength);

            builder.HasIndex(e => e.EntityChangeId);
        }
    }
}
