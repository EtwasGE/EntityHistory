using System;
using EntityHistory.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityHistory.EntityFrameworkCore.Common.Configurations
{
    public class EntityChangeSetConfig<TEntityChangeSet, TKey, TUser> : EntityChangeSetConfig<TEntityChangeSet, TKey>
        where TEntityChangeSet : EntityChangeSet<TKey, TUser>
        where TKey : struct, IEquatable<TKey>
        where TUser : class
    {
        public override void Configure(EntityTypeBuilder<TEntityChangeSet> builder)
        {
            base.Configure(builder);

            builder.HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId);
        }
    }

    public class EntityChangeSetConfig<TEntityChangeSet, TKey> : IEntityTypeConfiguration<TEntityChangeSet>
        where TEntityChangeSet : EntityChangeSet<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        public virtual void Configure(EntityTypeBuilder<TEntityChangeSet> builder)
        {
            builder.ToTable("EntityHistoryEntityChangeSets");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.BrowserInfo).HasMaxLength(EntityChangeSet<TKey>.MaxBrowserInfoLength);
            builder.Property(p => p.ClientIpAddress).HasMaxLength(EntityChangeSet<TKey>.MaxClientIpAddressLength);
            builder.Property(p => p.ClientName).HasMaxLength(EntityChangeSet<TKey>.MaxClientNameLength);

            builder.HasMany(p => p.EntityChanges)
                .WithOne()
                .HasForeignKey(p => p.EntityChangeSetId);

            builder.HasIndex(e => new { e.UserId });
            builder.HasIndex(e => new { ChangeTime = e.CreationTime });
        }
    }
}
