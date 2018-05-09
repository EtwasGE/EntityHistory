using System;
using System.Threading.Tasks;
using EntityHistory.Abstractions;

namespace EntityHistory.Core.History
{
    public class NullHistoryDbContextHelper<TContext> : IHistoryDbContextHelper<TContext>
    {
        public static NullHistoryDbContextHelper<TContext> Instance { get; } = new NullHistoryDbContextHelper<TContext>();

        public async Task<int> SaveChangesAsync(TContext context, Func<Task<int>> baseSaveChanges)
        {
            return await baseSaveChanges.Invoke();
        }

        public int SaveChanges(TContext context, Func<int> baseSaveChanges)
        {
            return baseSaveChanges.Invoke();
        }
    }
}
