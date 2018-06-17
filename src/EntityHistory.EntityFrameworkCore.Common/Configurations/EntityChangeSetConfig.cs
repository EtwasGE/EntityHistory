using System;
using EntityHistory.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityHistory.EntityFrameworkCore.Common.Configurations
{
    internal class EntityChangeSetConfig<TEntityChangeSet, TUserKey, TUser> : EntityChangeSetConfig<TEntityChangeSet, TUserKey>
        where TEntityChangeSet : EntityChangeSet<TUserKey, TUser>
        where TUserKey : struct, IEquatable<TUserKey>
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

    internal class EntityChangeSetConfig<TEntityChangeSet, TUserKey> : IEntityTypeConfiguration<TEntityChangeSet>
        where TEntityChangeSet : EntityChangeSet<TUserKey>
        where TUserKey : struct, IEquatable<TUserKey>
    {
        public virtual void Configure(EntityTypeBuilder<TEntityChangeSet> builder)
        {
            builder.ToTable("EntityHistoryEntityChangeSets");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.BrowserInfo).HasMaxLength(EntityChangeSet<TUserKey>.MaxBrowserInfoLength);
            builder.Property(p => p.ClientIpAddress).HasMaxLength(EntityChangeSet<TUserKey>.MaxClientIpAddressLength);
            builder.Property(p => p.ClientName).HasMaxLength(EntityChangeSet<TUserKey>.MaxClientNameLength);

            builder.HasMany(p => p.EntityChanges)
                .WithOne()
                .HasForeignKey(p => p.EntityChangeSetId);

            builder.HasIndex(e => new { e.UserId });
            builder.HasIndex(e => new { ChangeTime = e.CreationTime });
        }
    }
}
