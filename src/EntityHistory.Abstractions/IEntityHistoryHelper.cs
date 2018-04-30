using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityHistory.Abstractions
{
    public interface IEntityHistoryHelper<TEntityEntry, TEntityChangeSet>
    {
        TEntityChangeSet GetEntityChangeSet(ICollection<TEntityEntry> entityEntries);

        Task UpdateAndSaveAsync(TEntityChangeSet changeSet);
    }
}
