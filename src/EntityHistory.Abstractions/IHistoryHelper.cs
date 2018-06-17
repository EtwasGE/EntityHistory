using System.Collections.Generic;

namespace EntityHistory.Abstractions
{
    public interface IHistoryHelper<TEntityEntry, TEntityChangeSet>
    {
        TEntityChangeSet GetEntityChangeSet(ICollection<TEntityEntry> entityEntries);

        void UpdateEntityChangeSet(TEntityChangeSet changeSet);
    }
}
