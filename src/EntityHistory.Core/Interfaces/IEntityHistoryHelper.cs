using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityHistory.Core.Entities;

namespace EntityHistory.Core.Interfaces
{
    public interface IEntityHistoryHelper<TEntityEntry, TEntityChangeSet, TPrimaryKey>
        where TEntityChangeSet : EntityChangeSet<TPrimaryKey> 
        where TPrimaryKey : struct, IEquatable<TPrimaryKey>

    {
        TEntityChangeSet CreateEntityChangeSet(ICollection<TEntityEntry> entityEntries);
        Task SaveAsync(TEntityChangeSet changeSet);
    }
}
