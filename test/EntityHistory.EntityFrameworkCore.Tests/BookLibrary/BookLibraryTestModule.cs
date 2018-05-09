using Autofac;
using EntityHistory.EntityFrameworkCore.Common;
using EntityHistory.EntityFrameworkCore.Common.Interfaces;
using EntityHistory.EntityFrameworkCore.Tests.BookLibrary.Domain;

namespace EntityHistory.EntityFrameworkCore.Tests.BookLibrary
{
    public class BookLibraryTestModule : EntityFrameworkCoreTestModuleBase
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var options = GetDbContextOptions<BookLibraryDbContext>(builder);
            var isCreated = new BookLibraryDbContext(options).Database.EnsureCreated();

            builder.RegisterType<BookLibraryHistoryConfiguration>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<BookLibraryDbContext>()
                .AsSelf()
                .As<IDbContext>()
                .PropertiesAutowired();

            builder.RegisterType<BookLibraryHistoryHelper>()
                .AsImplementedInterfaces()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .InstancePerLifetimeScope();

            builder.RegisterType<HistoryDbContextHelper<CustomEntityChangeSet, long>>()
                .AsImplementedInterfaces();
        }
    }
}
