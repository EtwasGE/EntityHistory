using System.Threading.Tasks;
using EntityHistory.Abstractions;

namespace EntityHistory.Core.History
{
    public sealed class NullEntityHistoryStore : IEntityHistoryStore
    {
        public static NullEntityHistoryStore Instance { get; } = new NullEntityHistoryStore();

        public Task SaveAsync<TEntityChangeSet>(TEntityChangeSet entityChangeSet) where TEntityChangeSet : class
        {
            return Task.FromResult(0);
        }
    }
}
