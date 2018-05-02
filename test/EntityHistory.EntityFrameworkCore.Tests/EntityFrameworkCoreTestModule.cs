using Autofac;
using EntityHistory.EntityFrameworkCore.Common;
using EntityHistory.EntityFrameworkCore.Common.Interfaces;
using EntityHistory.EntityFrameworkCore.Tests.Ef;
using EntityHistory.EntityFrameworkCore.Tests.EntityHistory;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EntityHistory.EntityFrameworkCore.Tests
{
    public class EntityFrameworkCoreTestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterBloggingDbContextToSqliteInMemoryDb(builder);

            builder.RegisterType<BloggingEntityHistoryConfiguration>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<EntityHistoryStore>()
                .AsImplementedInterfaces();

            builder.RegisterType<EntityHistoryHelper>()
                .AsImplementedInterfaces()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .InstancePerLifetimeScope();

            builder.RegisterType<BloggingDbContext>()
                .AsSelf()
                .As<IDbContext>()
                .PropertiesAutowired();
        }

        private static void RegisterBloggingDbContextToSqliteInMemoryDb(ContainerBuilder builder)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BloggingDbContext>();

            var inMemorySqlite = new SqliteConnection("Data Source=:memory:");
            optionsBuilder.UseSqlite(inMemorySqlite);

            builder.Register(x => optionsBuilder.Options)
                .SingleInstance();

            inMemorySqlite.Open();

            var isCreated = new BloggingDbContext(optionsBuilder.Options).Database.EnsureCreated();
        }
    }
}
