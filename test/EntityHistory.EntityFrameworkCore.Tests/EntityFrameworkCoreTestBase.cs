using System;
using System.Threading.Tasks;
using Autofac;
using EntityHistory.EntityFrameworkCore.Tests.Domain;
using EntityHistory.EntityFrameworkCore.Tests.Ef;
using EntityHistory.TestBase;

namespace EntityHistory.EntityFrameworkCore.Tests
{
    public abstract class EntityFrameworkCoreTestBase : IntegratedTestBase<EntityFrameworkCoreTestModule>
    {
        protected EntityFrameworkCoreTestBase()
        {
            CreateInitialData();
        }

        private void CreateInitialData()
        {
            UsingDbContext(
                context =>
                {
                    var blog1 = new Blog("test-blog-1", "http://testblog1.myblogs.com");

                    context.Blogs.Add(blog1);
                    context.SaveChanges();

                    var post1 = new Post { Blog = blog1, Title = "test-post-1-title", Body = "test-post-1-body" };
                    var post2 = new Post { Blog = blog1, Title = "test-post-2-title", Body = "test-post-2-body" };
                    var post3 = new Post { Blog = blog1, Title = "test-post-3-title", Body = "test-post-3-body" };
                    var post4 = new Post { Blog = blog1, Title = "test-post-4-title", Body = "test-post-4-body" };

                    context.Posts.AddRange(post1, post2, post3, post4);
                });
        }

        public void UsingDbContext(Action<BloggingDbContext> action)
        {
            using (var context = Container.Resolve<BloggingDbContext>())
            {
                action(context);
                context.SaveChanges();
            }
        }

        public T UsingDbContext<T>(Func<BloggingDbContext, T> func)
        {
            T result;

            using (var context = Container.Resolve<BloggingDbContext>())
            {
                result = func(context);
                context.SaveChanges();
            }

            return result;
        }

        public async Task UsingDbContextAsync(Func<BloggingDbContext, Task> action)
        {
            using (var context = Container.Resolve<BloggingDbContext>())
            {
                await action(context);
                await context.SaveChangesAsync(true);
            }
        }

        public async Task<T> UsingDbContextAsync<T>(Func<BloggingDbContext, Task<T>> func)
        {
            T result;

            using (var context = Container.Resolve<BloggingDbContext>())
            {
                result = await func(context);
                context.SaveChanges();
            }

            return result;
        }
    }
}
