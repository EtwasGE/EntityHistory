using System;
using System.Threading;
using System.Threading.Tasks;
using EntityHistory.Abstractions;
using EntityHistory.Core.Entities;
using EntityHistory.Core.History;
using EntityHistory.EntityFrameworkCore.Common.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EntityHistory.EntityFrameworkCore.Identity
{
    public abstract class HistoryDbContextBase<TEntityChangeSet, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
        : IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
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
        public IHistoryDbContextHelper<DbContext> DbContextHelper { protected get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        protected HistoryDbContextBase(DbContextOptions options) : base(options)
        {
            DbContextHelper = NullHistoryDbContextHelper<DbContext>.Instance;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        protected HistoryDbContextBase()
        {
            DbContextHelper = NullHistoryDbContextHelper<DbContext>.Instance;
        }

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
        
        public override int SaveChanges()
        {
            return this.SaveChanges(acceptAllChangesOnSuccess: true);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return DbContextHelper.SaveChanges(this, () => base.SaveChanges(acceptAllChangesOnSuccess));
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.SaveChangesAsync(acceptAllChangesOnSuccess: true, cancellationToken: cancellationToken);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            return await DbContextHelper.SaveChangesAsync(this, () => base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken));
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
