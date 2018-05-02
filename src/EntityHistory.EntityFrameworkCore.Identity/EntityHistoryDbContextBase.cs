using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityHistory.Core.Entities;
using EntityHistory.Core.Extensions;
using EntityHistory.EntityFrameworkCore.Common.Configurations;
using EntityHistory.EntityFrameworkCore.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EntityHistory.EntityFrameworkCore.Identity
{
    public abstract class EntityHistoryDbContextBase<TEntityChangeSet, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
        : IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>, IDbContext
        where TEntityChangeSet : EntityChangeSet<TKey, TUser>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : struct, IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserLogin : IdentityUserLogin<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>
        where TUserToken : IdentityUserToken<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        protected EntityHistoryDbContextBase(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        protected EntityHistoryDbContextBase() { }

        /// <summary>
        /// Entity change sets.
        /// </summary>
        public virtual DbSet<TEntityChangeSet> EntityChangeSets { get; set; }

        /// <summary>
        /// Entity changes.
        /// </summary>
        public virtual DbSet<EntityChange> EntityChanges { get; set; }

        /// <summary>
        /// Entity property changes.
        /// </summary>
        public virtual DbSet<EntityPropertyChange> EntityPropertyChanges { get; set; }

        public IEntityHistoryHelper<TEntityChangeSet> EntityHistoryHelper { protected get; set; }

        public override int SaveChanges()
        {
            return this.SaveChanges(acceptAllChangesOnSuccess: true);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var changeSet = EntityHistoryHelper?.GetEntityChangeSet(ChangeTracker.Entries().ToList());
            
            var result = base.SaveChanges(acceptAllChangesOnSuccess);

            if (changeSet != null)
            {
                EntityHistoryHelper?.UpdateAndSave(changeSet);
            }

            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.SaveChangesAsync(acceptAllChangesOnSuccess: true, cancellationToken: cancellationToken);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            var changeSet = EntityHistoryHelper?.GetEntityChangeSet(ChangeTracker.Entries().ToList());

            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            if (EntityHistoryHelper != null && changeSet != null)
            {
                await EntityHistoryHelper.UpdateAndSaveAsync(changeSet);
            }

            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new EntityChangeSetConfig<TEntityChangeSet, TKey>());
            modelBuilder.ApplyConfiguration(new EntityChangeConfig<EntityChange>());
            modelBuilder.ApplyConfiguration(new EntityPropertyChangeConfig<EntityPropertyChange>());
        }
    }
}
