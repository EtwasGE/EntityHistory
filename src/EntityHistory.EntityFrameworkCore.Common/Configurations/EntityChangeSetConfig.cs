using System;
using EntityHistory.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityHistory.EntityFrameworkCore.Common.Configurations
{
    public class EntityChangeSetConfig<TEntityChangeSet, TPrimaryKey, TUser> : EntityChangeSetConfig<TEntityChangeSet, TPrimaryKey>
        where TEntityChangeSet : EntityChangeSet<TPrimaryKey, TUser>
        where TPrimaryKey : struct, IEquatable<TPrimaryKey>
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

    public class EntityChangeSetConfig<TEntityChangeSet, TPrimaryKey> : IEntityTypeConfiguration<TEntityChangeSet>
        where TEntityChangeSet : EntityChangeSet<TPrimaryKey> 
        where TPrimaryKey : struct, IEquatable<TPrimaryKey>
    {
        /// <summary>
        /// Maximum length of <see cref="EntityChangeSet{TPrimaryKey}.BrowserInfo"/> property.
        /// </summary>
        public const int MaxBrowserInfoLength = 512;

        /// <summary>
        /// Maximum length of <see cref="EntityChangeSet{TPrimaryKey}.ClientIpAddress"/> property.
        /// </summary>
        public const int MaxClientIpAddressLength = 64;

        /// <summary>
        /// Maximum length of <see cref="EntityChangeSet{TPrimaryKey}.ClientName"/> property.
        /// </summary>
        public const int MaxClientNameLength = 128;

        public virtual void Configure(EntityTypeBuilder<TEntityChangeSet> builder)
        {
            builder.ToTable("EntityHistoryEntityChangeSets");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.BrowserInfo).HasMaxLength(MaxBrowserInfoLength);
            builder.Property(p => p.ClientIpAddress).HasMaxLength(MaxClientIpAddressLength);
            builder.Property(p => p.ClientName).HasMaxLength(MaxClientNameLength);

            builder.HasMany(p => p.EntityChanges)
                .WithOne()
                .HasForeignKey(p => p.EntityChangeSetId);

            builder.HasIndex(e => new { e.UserId });
            builder.HasIndex(e => new { e.ChangeTime });
        }
    }
}
