using System.Linq;
using Autofac;
using EntityHistory.Abstractions;
using EntityHistory.Abstractions.Configuration;
using EntityHistory.Core.Entities;
using EntityHistory.EntityFrameworkCore.Tests.Ef;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Xunit;
using Shouldly;

namespace EntityHistory.EntityFrameworkCore.Tests.Tests
{
    public class Resolve_Tests : EntityFrameworkCoreTestBase
    {
        [Fact]
        public void Should_Resolve_BloggerDbContext_If_Registered()
        {
            Container.TryResolve<BloggingDbContext>(out var bloggingDbContext);
            bloggingDbContext.ShouldNotBeNull();

            var posts = bloggingDbContext.Posts.ToList();
            posts.Any().ShouldBeTrue();

            var entityChildSets = bloggingDbContext.EntityChangeSets.ToList();
            entityChildSets.Any().ShouldBeFalse();
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
            Container.TryResolve<IEntityHistoryHelper<EntityEntry, EntityChangeSet<long>>>(out var helper);
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
