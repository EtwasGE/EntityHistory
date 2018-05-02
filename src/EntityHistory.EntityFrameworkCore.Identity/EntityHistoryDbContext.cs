using System;
using EntityHistory.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EntityHistory.EntityFrameworkCore.Identity
{
    public class EntityHistoryDbContext : EntityHistoryDbContext<IdentityUser<long>, IdentityRole<long>, long>
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public EntityHistoryDbContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        protected EntityHistoryDbContext() { }
    }

    public class EntityHistoryDbContext<TUser, TRole, TKey>
        : EntityHistoryDbContext<EntityChangeSet<TKey, TUser>, TUser, TRole, TKey>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public EntityHistoryDbContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        protected EntityHistoryDbContext() { }
    }

    public class EntityHistoryDbContext<TEntityChangeSet, TKey>
        : EntityHistoryDbContext<TEntityChangeSet, IdentityUser<TKey>, IdentityRole<TKey>, TKey>
        where TEntityChangeSet : EntityChangeSet<TKey, IdentityUser<TKey>>
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public EntityHistoryDbContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        protected EntityHistoryDbContext() { }
    }

    public class EntityHistoryDbContext<TEntityChangeSet, TUser, TRole, TKey>
        : EntityHistoryDbContextBase<TEntityChangeSet, TUser, TRole, TKey, 
            IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>>
        where TEntityChangeSet : EntityChangeSet<TKey, TUser>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public EntityHistoryDbContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        protected EntityHistoryDbContext() { }
    }
}
