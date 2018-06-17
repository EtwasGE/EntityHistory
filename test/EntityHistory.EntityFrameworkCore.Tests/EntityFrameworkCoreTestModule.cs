﻿using System;
using Autofac;
using EntityHistory.Core.Entities;
using EntityHistory.EntityFrameworkCore.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EntityHistory.EntityFrameworkCore.Tests
{
    public class EntityFrameworkCoreTestModule<TUserKey, TUser> : EntityFrameworkCoreTestModuleBase<EntityChangeSet<TUserKey, TUser>, TUserKey>
        where TUserKey : struct, IEquatable<TUserKey>
        where TUser : class
    {
    }

    public class EntityFrameworkCoreTestModule : EntityFrameworkCoreTestModule<long>
    {
    }

    public class EntityFrameworkCoreTestModule<TUserKey> : EntityFrameworkCoreTestModuleBase<EntityChangeSet<TUserKey>, TUserKey>
        where TUserKey : struct, IEquatable<TUserKey>
    {
    }
    
    public abstract class EntityFrameworkCoreTestModuleBase<TEntityChangeSet, TUserKey> : Module
        where TEntityChangeSet : EntityChangeSet<TUserKey>, new()
        where TUserKey : struct, IEquatable<TUserKey>
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HistoryHelper<TEntityChangeSet, TUserKey>>()
                .AsImplementedInterfaces();

            builder.RegisterType<HistoryDbContextHelper<TEntityChangeSet, TUserKey>>()
                .AsImplementedInterfaces();
        }

        protected static DbContextOptions<TContext> GetDbContextOptions<TContext>(ContainerBuilder builder)
            where TContext : DbContext
        {
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();

            var inMemorySqlite = new SqliteConnection("Data Source=:memory:");
            optionsBuilder.UseSqlite(inMemorySqlite);

            builder.Register(x => optionsBuilder.Options)
                .SingleInstance();

            inMemorySqlite.Open();

            return optionsBuilder.Options;
        }
    }
}
