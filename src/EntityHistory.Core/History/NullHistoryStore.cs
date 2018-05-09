using System.Threading.Tasks;
using EntityHistory.Abstractions;

namespace EntityHistory.Core.History
{
    public sealed class NullHistoryStore : IHistoryStore
    {
        public static NullHistoryStore Instance { get; } = new NullHistoryStore();

        public Task SaveAsync<TEntityChangeSet>(TEntityChangeSet entityChangeSet) where TEntityChangeSet : class
        {
            return Task.FromResult(0);
        }
    }
}
