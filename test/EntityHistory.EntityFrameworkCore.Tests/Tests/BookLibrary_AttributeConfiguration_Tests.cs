using System.Linq;
using Autofac;
using EntityHistory.Core;
using EntityHistory.EntityFrameworkCore.Tests.BookLibrary;
using EntityHistory.EntityFrameworkCore.Tests.BookLibrary.Domain;
using Shouldly;
using Xunit;

namespace EntityHistory.EntityFrameworkCore.Tests.Tests
{
    public class AttributeConfiguration_Tests : EntityFrameworkCoreTestBase<BookLibraryTestModule>
    {
        [Fact]
        public void Book_Original_Title_And_Description_Test()
        {
            var context = Container.Resolve<BookLibraryDbContext>();

            var book = new Book
            {
                Title = "Original Title",
                Description = "Original Description"
            };

            context.Books.Add(book);
            context.SaveChanges();

            var propertyChanges = context.EntityChanges
                .Where(x => x.ChangeType == EntityChangeType.Created && x.EntityTypeFullName == book.GetType().FullName)
                .SelectMany(x => x.PropertyChanges).ToList();

            propertyChanges.SingleOrDefault(x =>
                    x.PropertyName == nameof(book.Title)
                    && x.OriginalValue == null
                    && x.NewValue == book.Title)
                .ShouldNotBeNull();

            propertyChanges.SingleOrDefault(x =>
                    x.PropertyName == nameof(book.Description)
                    && x.OriginalValue == null
                    && x.NewValue == book.Description)
                .ShouldNotBeNull();
        }

        [Fact]
        public void Author_Ignore_Custom_And_Any_Original_Test()
        {
            var context = Container.Resolve<BookLibraryDbContext>();

            var author = new Author
            {
                Name = "Author Name",
                Custom = "Custom"
            };

            context.Authors.Add(author);
            context.SaveChanges();

            var propertyChanges = context.EntityChanges
                .Where(x => x.ChangeType == EntityChangeType.Created && x.EntityTypeFullName == author.GetType().FullName)
                .SelectMany(x => x.PropertyChanges).ToList();

            propertyChanges.FirstOrDefault(x =>
                    x.PropertyName == nameof(author.Name)
                    && x.OriginalValue == null
                    && x.NewValue == author.Name)
                .ShouldNotBeNull();

            propertyChanges
                .FirstOrDefault(x => x.PropertyName == nameof(author.Custom))
                .ShouldBeNull();
        }

        [Fact]
        public void User_Ignore_Name_And_Override_Password_Test()
        {
            var context = Container.Resolve<BookLibraryDbContext>();

            var user = new User
            {
                Name = "User Name",
                Password = "Custom Password"
            };

            context.Users.Add(user);
            context.SaveChanges();

            var propertyChanges = context.EntityChanges
                .Where(x => x.ChangeType == EntityChangeType.Created && x.EntityTypeFullName == user.GetType().FullName)
                .SelectMany(x => x.PropertyChanges).ToList();

            propertyChanges.FirstOrDefault(x => x.PropertyName == nameof(user.Name))
                .ShouldBeNull();

            propertyChanges.FirstOrDefault(x =>
                    x.PropertyName == nameof(user.Password)
                    && x.OriginalValue == null
                    && x.NewValue == null)
                .ShouldNotBeNull();
        }

        [Fact]
        public void Tag_Base_Class_Test()
        {
            var context = Container.Resolve<BookLibraryDbContext>();

            var tag = new Tag
            {
                Name = "Tag Name",
                Custom = "Custom"
            };

            context.Tags.Add(tag);
            context.SaveChanges();

            var propertyChanges = context.EntityChanges
                .Where(x => x.ChangeType == EntityChangeType.Created && x.EntityTypeFullName == tag.GetType().FullName)
                .SelectMany(x => x.PropertyChanges).ToList();

            propertyChanges.FirstOrDefault(x =>
                    x.PropertyName == nameof(tag.Name)
                    && x.OriginalValue == null
                    && x.NewValue == tag.Name)
                .ShouldNotBeNull();

            propertyChanges.FirstOrDefault(x =>
                    x.PropertyName == nameof(tag.Custom)
                    && x.OriginalValue == null
                    && x.NewValue == "New Custom")
                .ShouldNotBeNull();
        }
    }
}
