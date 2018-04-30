using System;
using EntityHistory.Core.Entities;
using EntityHistory.EntityFrameworkCore.Common.Configurations;
using Microsoft.EntityFrameworkCore;

namespace EntityHistory.EntityFrameworkCore.Principal
{
    public class EntityHistoryDbContext<TUser> : EntityHistoryDbContext<long, TUser>
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

    public class EntityHistoryDbContext<TKey, TUser>
        : EntityHistoryDbContext<EntityChangeSet<TKey, TUser>, EntityChange<TKey>, EntityPropertyChange<TKey>, TKey, TUser>
        where TKey : struct, IEquatable<TKey>
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

    public class EntityHistoryDbContext<TEntityChangeSet, TEntityChange, TEntityPropertyChange, TKey, TUser>
        : EntityHistoryDbContextBase<TEntityChangeSet, TEntityChange, TEntityPropertyChange, TKey>
        where TEntityChangeSet : EntityChangeSet<TKey, TUser>
        where TEntityChange : EntityChange<TKey>
        where TEntityPropertyChange : EntityPropertyChange<TKey>
        where TKey : struct, IEquatable<TKey>
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new EntityChangeSetConfig<TEntityChangeSet, TKey, TUser>());
        }
    }
}

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

    public class EntityHistoryDbContext<TKey>
        : EntityHistoryDbContextBase<EntityChangeSet<TKey>, EntityChange<TKey>, EntityPropertyChange<TKey>, TKey>
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