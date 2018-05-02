using Autofac;
using EntityHistory.Abstractions;
using EntityHistory.Abstractions.Configuration;
using EntityHistory.EntityFrameworkCore.Common.Interfaces;
using EntityHistory.EntityFrameworkCore.Tests.BookLibrary;
using EntityHistory.EntityFrameworkCore.Tests.BookLibrary.Domain;
using Xunit;
using Shouldly;

namespace EntityHistory.EntityFrameworkCore.Tests.Tests
{
    public class BookLibrary_Resolve_Tests : EntityFrameworkCoreTestBase<BookLibraryTestModule>
    {
        [Fact]
        public void Should_Resolve_BookLibraryDbContext_If_Registered()
        {
            Container.TryResolve<BookLibraryDbContext>(out var bloggingDbContext);
            bloggingDbContext.ShouldNotBeNull();
        }

        [Fact]
        public void Should_Resolve_DbContext_If_Registered()
        {
            Container.TryResolve<IDbContext>(out var dbContext);
            dbContext.ShouldNotBeNull();
        }

        [Fact]
        public void Should_Resolve_EntityHistoryConfiguration_If_Registered()
        {
            Container.TryResolve<IEntityHistoryConfiguration>(out var config);
            config.ShouldNotBeNull();
        }

        [Fact]
        public void Should_Resolve_EntityHistoryHelper_If_Registered()
        {
            Container.TryResolve<IEntityHistoryHelper<CustomEntityChangeSet>>(out var helper);
            helper.ShouldNotBeNull();
        }

        [Fact]
        public void Should_Resolve_EntityHistoryStore_If_Registered()
        {
            Container.TryResolve<IEntityHistoryStore>(out var store);
            store.ShouldNotBeNull();
        }
    }
}
