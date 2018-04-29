using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityHistory.Core.Entities;
using EntityHistory.Core.Extensions;
using EntityHistory.Core.Interfaces;
using EntityHistory.EntityFrameworkCore.Common.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EntityHistory.EntityFrameworkCore.Principal
{
    public abstract class EntityHistoryDbContext<TUser>
        : EntityHistoryDbContext<long, TUser>
        where TUser : class
    {
    }

    public abstract class EntityHistoryDbContext<TPrimaryKey, TUser>
        : EntityHistoryDbContext<EntityChangeSet<TPrimaryKey, TUser>, EntityChange<TPrimaryKey>, EntityPropertyChange<TPrimaryKey>, TPrimaryKey, TUser>
        where TPrimaryKey : struct, IEquatable<TPrimaryKey>
        where TUser : class
    {
    }

    public abstract class EntityHistoryDbContext<TEntityChangeSet, TEntityChange, TEntityPropertyChange, TPrimaryKey, TUser>
        : EntityHistoryDbContext<TEntityChangeSet, TEntityChange, TEntityPropertyChange, TPrimaryKey>
        where TEntityChangeSet : EntityChangeSet<TPrimaryKey, TUser>
        where TEntityChange : EntityChange<TPrimaryKey>
        where TEntityPropertyChange : EntityPropertyChange<TPrimaryKey>
        where TPrimaryKey : struct, IEquatable<TPrimaryKey>
        where TUser : class
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new EntityChangeSetConfig<TEntityChangeSet, TPrimaryKey, TUser>());
        }
    }
}

namespace EntityHistory.EntityFrameworkCore
{
    public abstract class EntityHistoryDbContext
         : EntityHistoryDbContext<long>
    {
    }

    public abstract class EntityHistoryDbContext<TPrimaryKey>
        : EntityHistoryDbContext<EntityChangeSet<TPrimaryKey>, EntityChange<TPrimaryKey>, EntityPropertyChange<TPrimaryKey>, TPrimaryKey>
        where TPrimaryKey : struct, IEquatable<TPrimaryKey>
    {
    }
    
    public abstract class EntityHistoryDbContext<TEntityChangeSet, TEntityChange, TEntityPropertyChange, TPrimaryKey>
        : DbContext
        where TEntityChangeSet : EntityChangeSet<TPrimaryKey>
        where TEntityChange : EntityChange<TPrimaryKey>
        where TEntityPropertyChange : EntityPropertyChange<TPrimaryKey>
        where TPrimaryKey : struct, IEquatable<TPrimaryKey>
    {
        /// <summary>
        /// Entity change sets.
        /// </summary>
        public virtual DbSet<TEntityChangeSet> EntityChangeSets { get; set; }

        /// <summary>
        /// Entity changes.
        /// </summary>
        public virtual DbSet<TEntityChange> EntityChanges { get; set; }

        /// <summary>
        /// Entity property changes.
        /// </summary>
        public virtual DbSet<TEntityPropertyChange> EntityPropertyChanges { get; set; }

        public IEntityHistoryHelper<EntityEntry, TEntityChangeSet, TPrimaryKey> EntityHistoryHelper { get; set; }

        public override int SaveChanges()
        {
            return this.SaveChanges(acceptAllChangesOnSuccess: true);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var changeSet = EntityHistoryHelper?.CreateEntityChangeSet(ChangeTracker.Entries().ToList());

            var result = base.SaveChanges(acceptAllChangesOnSuccess);

            EntityHistoryHelper?.Save(changeSet);

            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.SaveChangesAsync(acceptAllChangesOnSuccess: true, cancellationToken: cancellationToken);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            var changeSet = EntityHistoryHelper?.CreateEntityChangeSet(ChangeTracker.Entries().ToList());

            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            if (EntityHistoryHelper != null)
            {
                await EntityHistoryHelper.SaveAsync(changeSet);
            }

            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new EntityChangeSetConfig<TEntityChangeSet, TPrimaryKey>());
            modelBuilder.ApplyConfiguration(new EntityChangeConfig<TEntityChange, TPrimaryKey>());
            modelBuilder.ApplyConfiguration(new EntityPropertyChangeConfig<TEntityPropertyChange, TPrimaryKey>());
        }
    }
}