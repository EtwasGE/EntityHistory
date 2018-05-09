using EntityHistory.Abstractions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EntityHistory.EntityFrameworkCore.Common.Interfaces
{
    public interface IHistoryHelper<TEntityChangeSet> : IHistoryHelper<EntityEntry, TEntityChangeSet>
    {
    }
}
