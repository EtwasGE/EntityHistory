using Autofac;
using EntityHistory.EntityFrameworkCore.Common;
using EntityHistory.EntityFrameworkCore.Common.Interfaces;

namespace EntityHistory.EntityFrameworkCore.Tests.Blogging
{
    public class BloggingTestModule : EntityFrameworkCoreTestModuleBase
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var options = GetDbContextOptions<BloggingDbContext>(builder);
            var isCreated = new BloggingDbContext(options).Database.EnsureCreated();

            builder.RegisterType<BloggingEntityHistoryConfiguration>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<BloggingDbContext>()
                .AsSelf()
                .As<IDbContext>()
                .PropertiesAutowired();

            builder.RegisterType<EntityHistoryHelper>()
                .AsImplementedInterfaces()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .InstancePerLifetimeScope();
        }
    }
}
