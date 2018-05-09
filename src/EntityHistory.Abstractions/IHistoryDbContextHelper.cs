using System;
using System.Threading.Tasks;

namespace EntityHistory.Abstractions
{
    public interface IHistoryDbContextHelper<in TContext>
    {
        Task<int> SaveChangesAsync(TContext context, Func<Task<int>> baseSaveChanges);

        int SaveChanges(TContext context, Func<int> baseSaveChanges);
    }
}
