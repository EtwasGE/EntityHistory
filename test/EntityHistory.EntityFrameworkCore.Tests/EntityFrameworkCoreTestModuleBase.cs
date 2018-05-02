using Autofac;
using EntityHistory.EntityFrameworkCore.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EntityHistory.EntityFrameworkCore.Tests
{
    public abstract class EntityFrameworkCoreTestModuleBase : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EntityHistoryStore>()
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
