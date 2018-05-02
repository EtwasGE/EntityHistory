using EntityHistory.Abstractions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EntityHistory.EntityFrameworkCore.Common.Interfaces
{
    public interface IEntityHistoryHelper<TEntityChangeSet> : IEntityHistoryHelper<EntityEntry, TEntityChangeSet>
    {
    }
}
