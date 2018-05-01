using System.Linq;
using Autofac;
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
            var context = Container.ResolveNamed<BloggingDbContext>("BloggingDbContext_with_BlogEntityHistoryConfiguration");

            var blog = context.Blogs.First();
            var oldName = blog.Name;
            blog.Name = "Custom blog name";
            blog.Url = "New URL";

            context.SaveChanges();

            var allEntityPropertyChanges = context.EntityPropertyChanges.ToList();
            
            var urlProperty = allEntityPropertyChanges.FirstOrDefault(x => x.PropertyName == "Url");
            urlProperty.ShouldBeNull();

            var nameProperty = allEntityPropertyChanges.SingleOrDefault(x =>
                x.PropertyName == "Name"
                && x.OriginalValue == oldName
                && x.NewValue == blog.Name); //"МИР ТРУД МАЙ"

            nameProperty.ShouldNotBeNull();
        }
    }
}
