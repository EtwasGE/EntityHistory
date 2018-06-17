using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using EntityHistory.TestBase;
using Microsoft.EntityFrameworkCore;

namespace EntityHistory.EntityFrameworkCore.TestBase
{
    public abstract class EntityFrameworkCoreTestBase<TStartModule> : TestBase<TStartModule>
        where TStartModule: IModule
    {
        public void UsingDbContext<TContext>(Action<TContext> action) where TContext : DbContext
        {
            using (var context = Container.Resolve<TContext>())
            {
                action(context);
                context.SaveChanges();
            }
        }

        public T UsingDbContext<TContext, T>(Func<TContext, T> func) where TContext : DbContext
        {
            T result;

            using (var context = Container.Resolve<TContext>())
            {
                result = func(context);
                context.SaveChanges();
            }

            return result;
        }

        public async Task UsingDbContextAsync<TContext>(Func<TContext, Task> action) where TContext : DbContext
        {
            using (var context = Container.Resolve<TContext>())
            {
                await action(context);
                await context.SaveChangesAsync(true);
            }
        }

        public async Task<T> UsingDbContextAsync<TContext, T>(Func<TContext, Task<T>> func) where TContext : DbContext
        {
            T result;

            using (var context = Container.Resolve<TContext>())
            {
                result = await func(context);
                context.SaveChanges();
            }

            return result;
        }
    }
}
