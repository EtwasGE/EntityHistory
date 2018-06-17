using System.Linq;
using Autofac;
using EntityHistory.Abstractions;
using EntityHistory.Core;
using EntityHistory.Core.Entities;
using EntityHistory.EntityFrameworkCore.Tests.Blogging;
using EntityHistory.EntityFrameworkCore.Tests.Blogging.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shouldly;
using Xunit;

namespace EntityHistory.EntityFrameworkCore.Tests.Tests
{
    public class Blogging_FluentApiConfiguration_Tests : EntityFrameworkCoreTestBase<BloggingTestModule>
    {
        public Blogging_FluentApiConfiguration_Tests()
        {
            CreateInitialData();
        }

        private void CreateInitialData()
        {
            UsingDbContext<BloggingDbContext>(
                context =>
                {
                    var blog1 = new Blog("test-blog-1", "http://testblog1.myblogs.com");

                    context.Blogs.Add(blog1);

                    var post1 = new Post { Blog = blog1, Title = "test-post-1-title", Body = "test-post-1-body" };
                    var post2 = new Post { Blog = blog1, Title = "test-post-2-title", Body = "test-post-2-body" };
                    var post3 = new Post { Blog = blog1, Title = "test-post-3-title", Body = "test-post-3-body" };
                    var post4 = new Post { Blog = blog1, Title = "test-post-4-title", Body = "test-post-4-body" };

                    context.Posts.AddRange(post1, post2, post3, post4);
                });
        }

        [Fact]
        public void Should_Resolve_HistoryHelper_If_Registered()
        {
            Container.TryResolve<IHistoryHelper<EntityEntry, EntityChangeSet<long>>>(out var helper);
            helper.ShouldNotBeNull();
        }

        [Fact]
        public void Should_Resolve_HistoryDbContext_If_Registered()
        {
            Container.TryResolve<IHistoryDbContextHelper<DbContext>>(out var helper);
            helper.ShouldNotBeNull();
        }

        [Fact]
        public void Should_Resolve_BloggerDbContext_If_Registered()
        {
            Container.TryResolve<BloggingDbContext>(out var bloggingDbContext);
            bloggingDbContext.ShouldNotBeNull();
        }

        [Fact]
        public void ForEntity_Override_Updated_Blog_Name_Test()
        {
            var context = Container.Resolve<BloggingDbContext>();

            var blog = context.Blogs.First();
            blog.Name = "Custom blog name";
            var overriddenValue = "Override value";

            context.SaveChanges();

            var updatedBlogPropertyChanges = context.EntityChanges
                .Where(x => x.ChangeType == EntityChangeType.Updated && x.EntityTypeFullName == blog.GetType().FullName)
                .SelectMany(x => x.PropertyChanges).ToList();

            updatedBlogPropertyChanges.SingleOrDefault(x =>
                    x.PropertyName == nameof(blog.Name)
                    && x.OriginalValue == overriddenValue
                    && x.NewValue == overriddenValue)
                .ShouldNotBeNull();
        }

        [Fact]
        public void ForEntity_Format_Create_Blog_Raiting_Test()
        {
            var context = Container.Resolve<BloggingDbContext>();

            var blog = new Blog
            {
                Raiting = 1234
            };
            
            context.SaveChanges();

            var propertyChanges = context.EntityChanges
                .Where(x => x.ChangeType == EntityChangeType.Created&& x.EntityTypeFullName == blog.GetType().FullName)
                .SelectMany(x => x.PropertyChanges).ToList();

            propertyChanges.SingleOrDefault(x =>
                    x.PropertyName == nameof(blog.Raiting)
                    && x.OriginalValue == null
                    && x.NewValue == "CustomValue")
                .ShouldNotBeNull();
        }

        [Fact]
        public void ForEntity_Ignore_Updated_Blog_Url_Test()
        {
            var context = Container.Resolve<BloggingDbContext>();

            var blog = context.Blogs.First();
            blog.Url = "New URL";

            context.SaveChanges();

            var updatedBlogPropertyChanges = context.EntityChanges
                .Where(x => x.ChangeType == EntityChangeType.Updated && x.EntityTypeFullName == blog.GetType().FullName)
                .SelectMany(x => x.PropertyChanges).ToList();

            updatedBlogPropertyChanges
                .FirstOrDefault(x => x.PropertyName == nameof(blog.Url))
                .ShouldBeNull();
        }

        [Fact]
        public void Original_Updated_Post_Title_Test()
        {
            var context = Container.Resolve<BloggingDbContext>();

            var post = context.Posts.First();
            var postOldTitle = post.Title;
            post.Title = "Custom post title";

            context.SaveChanges();

            var updatedPostPropertyChanges = context.EntityChanges
                .Where(x => x.ChangeType == EntityChangeType.Updated && x.EntityTypeFullName == post.GetType().FullName)
                .SelectMany(x => x.PropertyChanges).ToList();

            updatedPostPropertyChanges.SingleOrDefault(x =>
                    x.PropertyName == nameof(post.Title)
                    && x.OriginalValue == postOldTitle
                    && x.NewValue == post.Title)
                .ShouldNotBeNull();
        }

        [Fact]
        public void ForEntity_Format_Updated_Post_Body_Test()
        {
            var context = Container.Resolve<BloggingDbContext>();

            var post = context.Posts.First();

            var postOldBody = post.Body;
            post.Body = "Custom post body";
            var formatedValue = "Custom post body formated";

            context.SaveChanges();

            var updatedPostPropertyChanges = context.EntityChanges
                .Where(x => x.ChangeType == EntityChangeType.Updated && x.EntityTypeFullName == post.GetType().FullName)
                .SelectMany(x => x.PropertyChanges).ToList();

            updatedPostPropertyChanges.SingleOrDefault(x =>
                    x.PropertyName == nameof(post.Body)
                    && x.OriginalValue == postOldBody
                    && x.NewValue == formatedValue)
                .ShouldNotBeNull();
        }

        [Fact]
        public void AllInclude_CommentBase_Created_Comments_Test()
        {
            var context = Container.Resolve<BloggingDbContext>();

            var firstComment = new CommentFirst
            {
                Body = "Comment First Body"
            };

            context.FirstComments.Add(firstComment);

            var secondComment = new CommentSecond
            {
                RatingScale = -5.5f
            };

            context.SecondComments.Add(secondComment);

            context.SaveChanges();

            var createdCommentPropertyChanges = context.EntityChanges
                .Where(x => x.ChangeType == EntityChangeType.Created
                            && (x.EntityTypeFullName == firstComment.GetType().FullName
                                || x.EntityTypeFullName == secondComment.GetType().FullName))
                .SelectMany(x => x.PropertyChanges).ToList();

            createdCommentPropertyChanges.SingleOrDefault(x =>
                    x.PropertyName == nameof(firstComment.Body)
                    && x.OriginalValue == null
                    && x.NewValue == firstComment.Body)
                .ShouldNotBeNull();

            createdCommentPropertyChanges.SingleOrDefault(x =>
                    x.PropertyName == nameof(secondComment.RatingScale)
                    && x.OriginalValue == null
                    && x.NewValue == secondComment.RatingScale.ToString())
                .ShouldNotBeNull();
        }
    }


}
