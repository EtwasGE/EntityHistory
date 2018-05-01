using EntityHistory.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityHistory.EntityFrameworkCore.Common.Configurations
{
    public class EntityPropertyChangeConfig<TEntityPropertyChange> : IEntityTypeConfiguration<TEntityPropertyChange>
        where TEntityPropertyChange: EntityPropertyChange
    {
        public void Configure(EntityTypeBuilder<TEntityPropertyChange> builder)
        {
            builder.ToTable("EntityHistoryEntityPropertyChanges");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.NewValue).HasMaxLength(EntityPropertyChange.MaxValueLength);
            builder.Property(p => p.OriginalValue).HasMaxLength(EntityPropertyChange.MaxValueLength);
            builder.Property(p => p.PropertyName).HasMaxLength(EntityPropertyChange.MaxPropertyNameLength);
            builder.Property(p => p.PropertyTypeFullName).HasMaxLength(EntityPropertyChange.MaxPropertyTypeFullNameLength);

            builder.HasIndex(e => e.EntityChangeId);
        }
    }
}
