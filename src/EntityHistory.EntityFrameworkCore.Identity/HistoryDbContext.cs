using System;
using EntityHistory.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EntityHistory.EntityFrameworkCore.Identity
{
    public class HistoryDbContext : HistoryDbContext<IdentityUser<long>, IdentityRole<long>, long>
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public HistoryDbContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        protected HistoryDbContext() { }
    }

    public class HistoryDbContext<TUser, TRole, TKey>
        : HistoryDbContext<EntityChangeSet<TKey, TUser>, TUser, TRole, TKey>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public HistoryDbContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        protected HistoryDbContext() { }
    }

    public class HistoryDbContext<TEntityChangeSet, TKey>
        : HistoryDbContext<TEntityChangeSet, IdentityUser<TKey>, IdentityRole<TKey>, TKey>
        where TEntityChangeSet : EntityChangeSet<TKey, IdentityUser<TKey>>
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public HistoryDbContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        protected HistoryDbContext() { }
    }

    public class HistoryDbContext<TEntityChangeSet, TUser, TRole, TKey>
        : HistoryDbContextBase<TEntityChangeSet, TUser, TRole, TKey, 
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
        public HistoryDbContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        protected HistoryDbContext() { }
    }
}
