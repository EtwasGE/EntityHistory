using System;
using EntityHistory.Core.Entities;
using Microsoft.EntityFrameworkCore;
namespace EntityHistory.EntityFrameworkCore
{
    public class HistoryDbContext : HistoryDbContext<long>
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

    public class HistoryDbContext<TUserKey>
        : HistoryDbContextBase<EntityChangeSet<TUserKey>, TUserKey>
        where TUserKey : struct, IEquatable<TUserKey>
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

    public class HistoryDbContext<TUserKey, TUser>
        : HistoryDbContextBase<EntityChangeSet<TUserKey, TUser>, TUserKey, TUser>
        where TUserKey : struct, IEquatable<TUserKey>
        where TUser : class
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