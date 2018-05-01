using Autofac;
using EntityHistory.Configuration;
using EntityHistory.EntityFrameworkCore.Common;
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

            builder.RegisterType<EntityHistoryConfiguration>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<EntityHistoryStore>()
                .AsImplementedInterfaces();

            builder.RegisterType<EntityHistoryHelper>()
                .AsImplementedInterfaces()
                .PropertiesAutowired();

            builder.RegisterType<BloggingDbContext>()
                .AsSelf()
                .As<IEntityHistoryDbContext>()
                .PropertiesAutowired();

            builder.RegisterType<BloggingDbContext>()
                .OnActivated(x =>
                {
                    var config = new BlogEntityHistoryConfiguration();
                    x.Instance.EntityHistoryHelper = new EntityHistoryHelper(config);
                })
                .Named<BloggingDbContext>("BloggingDbContext_with_BlogEntityHistoryConfiguration");
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
