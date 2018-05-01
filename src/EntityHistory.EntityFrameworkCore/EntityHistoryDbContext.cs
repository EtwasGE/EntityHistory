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

    public class EntityHistoryDbContext<TUserKey, TUser>
        : EntityHistoryDbContext<EntityChangeSet<TUserKey, TUser>, EntityChange, EntityPropertyChange, TUserKey, TUser>
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

    public class EntityHistoryDbContext<TEntityChangeSet, TEntityChange, TEntityPropertyChange, TUserKey, TUser>
        : EntityHistoryDbContextBase<TEntityChangeSet, TEntityChange, TEntityPropertyChange, TUserKey>
        where TEntityChangeSet : EntityChangeSet<TUserKey, TUser>
        where TEntityChange : EntityChange
        where TEntityPropertyChange : EntityPropertyChange
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new EntityChangeSetConfig<TEntityChangeSet, TUserKey, TUser>());
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

    public class EntityHistoryDbContext<TUserKey>
        : EntityHistoryDbContextBase<EntityChangeSet<TUserKey>, EntityChange, EntityPropertyChange, TUserKey>
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
}