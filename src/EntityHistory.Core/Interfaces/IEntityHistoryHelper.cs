using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityHistory.Core.Interfaces
{
    public interface IEntityHistoryHelper<TEntityEntry>
    {
        IEntityChangeSet CreateEntityChangeSet(ICollection<TEntityEntry> entityEntries);

        Task SaveAsync(IEntityChangeSet changeSet);
    }
}
