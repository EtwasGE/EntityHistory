using System.Linq;
using Autofac;
using EntityHistory.Core;
using EntityHistory.EntityFrameworkCore.Tests.Ef;
using Shouldly;
using Xunit;

namespace EntityHistory.EntityFrameworkCore.Tests.Tests
{
    public class EntityHistoryHelper_Tests : EntityFrameworkCoreTestBase
    {
        [Fact]
        public void Change_Blog_Name_Test()
        {
            var context = Container.Resolve<BloggingDbContext>();

            var blog = context.Blogs.First();
            var oldName = blog.Name;
            blog.Name = "Custom blog name";
            blog.Url = "New URL";

            context.SaveChanges();

            var allUpdatedPropertyChanges = context.EntityChanges
                .Where(x=>x.ChangeType == EntityChangeType.Updated)
                .SelectMany(x=>x.PropertyChanges).ToList();
            
            var urlProperty = allUpdatedPropertyChanges.FirstOrDefault(x => x.PropertyName == nameof(blog.Url));
            urlProperty.ShouldBeNull();

            var nameProperty = allUpdatedPropertyChanges.SingleOrDefault(x =>
                x.PropertyName == nameof(blog.Name)
                && x.OriginalValue == oldName
                && x.NewValue == blog.Name); //"МИР ТРУД МАЙ"

            nameProperty.ShouldNotBeNull();
        }
    }
}
