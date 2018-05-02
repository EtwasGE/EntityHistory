using Autofac;
using EntityHistory.EntityFrameworkCore.Common.Interfaces;

namespace EntityHistory.EntityFrameworkCore.Tests.BookLibrary
{
    public class BookLibraryTestModule : EntityFrameworkCoreTestModuleBase
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var options = GetDbContextOptions<BookLibraryDbContext>(builder);
            var isCreated = new BookLibraryDbContext(options).Database.EnsureCreated();

            builder.RegisterType<BookLibraryEntityHistoryConfiguration>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<BookLibraryDbContext>()
                .AsSelf()
                .As<IDbContext>()
                .PropertiesAutowired();

            builder.RegisterType<BookLibraryEntityHistoryHelper>()
                .AsImplementedInterfaces()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .InstancePerLifetimeScope();
        }
    }
}
