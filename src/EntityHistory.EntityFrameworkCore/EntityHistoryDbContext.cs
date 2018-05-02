using System;
using EntityHistory.Core.Entities;
using Microsoft.EntityFrameworkCore;
namespace EntityHistory.EntityFrameworkCore
{
    public class EntityHistoryDbContext : EntityHistoryDbContext<long>
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

    public class EntityHistoryDbContext<TUserKey>
        : EntityHistoryDbContextBase<EntityChangeSet<TUserKey>, TUserKey>
        where TUserKey : struct, IEquatable<TUserKey>
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

    public class EntityHistoryDbContext<TUserKey, TUser>
        : EntityHistoryDbContextBase<EntityChangeSet<TUserKey, TUser>, TUserKey, TUser>
        where TUserKey : struct, IEquatable<TUserKey>
        where TUser : class
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