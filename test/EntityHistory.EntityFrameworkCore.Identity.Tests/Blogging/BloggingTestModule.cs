using Autofac;
using EntityHistory.Abstractions.Configuration;
using EntityHistory.Configuration;
using EntityHistory.EntityFrameworkCore.Identity.Tests.Blogging.Domain;
using Microsoft.AspNetCore.Identity;

namespace EntityHistory.EntityFrameworkCore.Identity.Tests.Blogging
{
    public class BloggingTestModule : EntityFrameworkCoreTestModule<IdentityUser<long>>
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var options = GetDbContextOptions<BloggingDbContext>(builder);
            var isCreated = new BloggingDbContext(options).Database.EnsureCreated();

            builder.RegisterType<BloggingDbContext>()
                .PropertiesAutowired();

            InitialConfiguration();
        }

        private void InitialConfiguration()
        {
            HistoryConfiguration.IsEnabledForAnonymousUsers = true;

            HistoryConfiguration.Setup()
                .ForEntity<Post>(x => x.Format<string>(property => property.Body, body => $"{body} formated"))
                .AllInclude<CommentFirstBase>()
                .AllInclude(typeof(CommentSecondBase))
                .ApplyConfiguration(new BlogConfig())
                .ApplyConfiguration(new UserConfig());
        }
    }

    public class BlogConfig : IConfigurationModule<Blog>
    {
        public void Configure(IEntityConfiguration<Blog> config)
        {
            config
                .Ignore(x => x.Url)
                .Override(x => x.Name, "Override value")
                .Format(property => property.Raiting, raiting => "CustomValue");
        }
    }

    public class UserConfig : IConfigurationModule<IdentityUser<long>>
    {
        public void Configure(IEntityConfiguration<IdentityUser<long>> config)
        {
            config
                .Ignore(x => x.PasswordHash);
        }
    }
}
