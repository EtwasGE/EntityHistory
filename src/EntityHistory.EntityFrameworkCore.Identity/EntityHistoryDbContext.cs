using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityHistory.Core.Entities;
using EntityHistory.Core.Extensions;
using EntityHistory.Core.Interfaces;
using EntityHistory.EntityFrameworkCore.Common.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EntityHistory.EntityFrameworkCore.Identity
{
    public abstract class EntityHistoryDbContext
        : EntityHistoryDbContext<IdentityUser<long>, IdentityRole<long>, long>
    {
    }

    public abstract class EntityHistoryDbContext<TUser, TRole, TPrimaryKey>
        : EntityHistoryDbContext<EntityChangeSet<TPrimaryKey, TUser>, EntityChange<TPrimaryKey>, EntityPropertyChange<TPrimaryKey>, TUser, TRole, TPrimaryKey>
        where TUser : IdentityUser<TPrimaryKey>
        where TRole : IdentityRole<TPrimaryKey>
        where TPrimaryKey : struct, IEquatable<TPrimaryKey>
    {
    }

    public abstract class EntityHistoryDbContext<TEntityChangeSet, TEntityChange, TEntityPropertyChange, TPrimaryKey>
        : EntityHistoryDbContext<TEntityChangeSet, TEntityChange, TEntityPropertyChange, IdentityUser<TPrimaryKey>, IdentityRole<TPrimaryKey>, TPrimaryKey>
        where TEntityChangeSet : EntityChangeSet<TPrimaryKey, IdentityUser<TPrimaryKey>>
        where TEntityChange : EntityChange<TPrimaryKey>
        where TEntityPropertyChange : EntityPropertyChange<TPrimaryKey>
        where TPrimaryKey : struct, IEquatable<TPrimaryKey>
    {
    }

    public abstract class EntityHistoryDbContext<TEntityChangeSet, TEntityChange, TEntityPropertyChange, TUser, TRole, TPrimaryKey>
        : EntityHistoryDbContext<TEntityChangeSet, TEntityChange, TEntityPropertyChange, TUser, TRole, TPrimaryKey, 
            IdentityUserClaim<TPrimaryKey>, IdentityUserRole<TPrimaryKey>, IdentityUserLogin<TPrimaryKey>, IdentityRoleClaim<TPrimaryKey>, IdentityUserToken<TPrimaryKey>>
        where TEntityChangeSet : EntityChangeSet<TPrimaryKey, TUser>
        where TEntityChange : EntityChange<TPrimaryKey>
        where TEntityPropertyChange : EntityPropertyChange<TPrimaryKey>
        where TUser : IdentityUser<TPrimaryKey>
        where TRole : IdentityRole<TPrimaryKey>
        where TPrimaryKey : struct, IEquatable<TPrimaryKey>
    {
    }
    
    public abstract class EntityHistoryDbContext<TEntityChangeSet, TEntityChange, TEntityPropertyChange, TUser, TRole, TPrimaryKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
        : IdentityDbContext<TUser, TRole, TPrimaryKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
        where TEntityChangeSet : EntityChangeSet<TPrimaryKey, TUser>
        where TEntityChange : EntityChange<TPrimaryKey>
        where TEntityPropertyChange : EntityPropertyChange<TPrimaryKey>
        where TUser : IdentityUser<TPrimaryKey>
        where TRole : IdentityRole<TPrimaryKey>
        where TPrimaryKey : struct, IEquatable<TPrimaryKey>
        where TUserClaim : IdentityUserClaim<TPrimaryKey>
        where TUserRole : IdentityUserRole<TPrimaryKey>
        where TUserLogin : IdentityUserLogin<TPrimaryKey>
        where TRoleClaim : IdentityRoleClaim<TPrimaryKey>
        where TUserToken : IdentityUserToken<TPrimaryKey>
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
