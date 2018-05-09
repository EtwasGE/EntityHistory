using Autofac;
using EntityHistory.Abstractions;
using EntityHistory.Abstractions.Configuration;
using EntityHistory.Core.Entities;
using EntityHistory.EntityFrameworkCore.Common.Interfaces;
using EntityHistory.EntityFrameworkCore.Tests.Blogging;
using Xunit;
using Shouldly;

namespace EntityHistory.EntityFrameworkCore.Tests.Tests
{
    public class Blogging_Resolve_Tests : EntityFrameworkCoreTestBase<BloggingTestModule>
    {
        [Fact]
        public void Should_Resolve_BloggerDbContext_If_Registered()
        {
            Container.TryResolve<BloggingDbContext>(out var bloggingDbContext);
            bloggingDbContext.ShouldNotBeNull();
        }

        [Fact]
        public void Should_Resolve_DbContext_If_Registered()
        {
            Container.TryResolve<IDbContext>(out var dbContext);
            dbContext.ShouldNotBeNull();
        }

        [Fact]
        public void Should_Resolve_HistoryConfiguration_If_Registered()
        {
            Container.TryResolve<IHistoryConfiguration>(out var config);
            config.ShouldNotBeNull();
        }

        [Fact]
        public void Should_Resolve_HistoryHelper_If_Registered()
        {
            Container.TryResolve<IHistoryHelper<EntityChangeSet<long>>>(out var helper);
            helper.ShouldNotBeNull();
        }

        [Fact]
        public void Should_Resolve_HistoryStore_If_Registered()
        {
            Container.TryResolve<IHistoryStore>(out var store);
            store.ShouldNotBeNull();
        }

        [Fact]
        public void Should_Resolve_HistoryDbContext_If_Registered()
        {
            Container.TryResolve<IHistoryDbContextHelper>(out var helper);
            helper.ShouldNotBeNull();
        }
    }
}
