using System.Linq;
using Autofac;
using EntityHistory.Abstractions;
using EntityHistory.Core;
using EntityHistory.Core.Entities;
using EntityHistory.EntityFrameworkCore.Identity.Tests.BookLibrary;
using EntityHistory.EntityFrameworkCore.Identity.Tests.BookLibrary.Domain;
using EntityHistory.EntityFrameworkCore.TestBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shouldly;
using Xunit;

namespace EntityHistory.EntityFrameworkCore.Identity.Tests.Tests
{
    public class BookLibrary_AttributeConfiguration_Tests : EntityFrameworkCoreTestBase<BookLibraryTestModule>
    {
        [Fact]
        public void Should_Resolve_HistoryHelper_If_Registered()
        {
            Container.TryResolve<IHistoryHelper<EntityEntry, EntityChangeSet<long, CustomUser>>>(out var helper);
            helper.ShouldNotBeNull();
        }

        [Fact]
        public void Should_Resolve_HistoryDbContext_If_Registered()
        {
            Container.TryResolve<IHistoryDbContextHelper<DbContext>>(out var helper);
            helper.ShouldNotBeNull();
        }

        [Fact]
        public void Should_Resolve_BookLibraryDbContext_If_Registered()
        {
            Container.TryResolve<BookLibraryDbContext>(out var bloggingDbContext);
            bloggingDbContext.ShouldNotBeNull();
        }

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
        public void User_Test()
        {
            var context = Container.Resolve<BookLibraryDbContext>();

            var userName = "User Name";
            var passwordHash = "password hash";

            var user = new CustomUser
            {
                UserName = userName,
                PasswordHash = passwordHash
            };

            context.Users.Add(user);
            context.SaveChanges();

            var propertyChanges = context.EntityChanges
                .Where(x => x.ChangeType == EntityChangeType.Created && x.EntityTypeFullName == user.GetType().FullName)
                .SelectMany(x => x.PropertyChanges).ToList();

            propertyChanges.FirstOrDefault(x => x.PropertyName == nameof(user.UserName) && x.NewValue == userName)
                .ShouldNotBeNull();

            propertyChanges.FirstOrDefault(x => x.PropertyName == nameof(user.PasswordHash) && x.NewValue == passwordHash)
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
