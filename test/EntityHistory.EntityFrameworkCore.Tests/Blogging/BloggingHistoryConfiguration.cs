using EntityHistory.Abstractions.Configuration;
using EntityHistory.Configuration;
using EntityHistory.EntityFrameworkCore.Tests.Blogging.Domain;

namespace EntityHistory.EntityFrameworkCore.Tests.Blogging
{
    public class BloggingHistoryConfiguration : HistoryConfiguration
    {
        public BloggingHistoryConfiguration()
        {
            IsEnabledForAnonymousUsers = true;
        }

        protected override void OnRegistration(IEntitiesConfigurator config)
        {
            //config.AllInclude<object>();
            config.ForEntity<Post>(x => x.Format<string>(property => property.Body, body => $"{body} formated"));
            config.AllInclude<CommentFirstBase>();
            config.AllInclude(typeof(CommentSecondBase));
            config.ApplyConfiguration(new BlogConfig());
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
}
