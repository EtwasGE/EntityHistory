using EntityHistory.Abstractions.Configuration;
using EntityHistory.Configuration;
using EntityHistory.EntityFrameworkCore.Tests.Domain;

namespace EntityHistory.EntityFrameworkCore.Tests.EntityHistory
{
    public class BlogEntityHistoryConfiguration : EntityHistoryConfiguration
    {
        public BlogEntityHistoryConfiguration()
        {
            IsEnabledForAnonymousUsers = true;
        }

        protected override void OnEntityConfig(IEntitiesConfigurator config)
        {
            //config.AllInclude<Blog>();
            config.ApplyConfiguration(new CustomBlogConfig());
        }
    }

    public class CustomBlogConfig : IConfigurationModule<Blog>
    {
        public void Configure(IEntityConfiguration<Blog> config)
        {
            config
                .Ignore(x => x.Url)
                .Override(x => x.Name, "МИР ТРУД МАЙ");
        }
    }
}
