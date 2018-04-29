using System;
using System.Threading.Tasks;
using EntityHistory.Core.Entities;

namespace EntityHistory.Core.Interfaces
{
    /// <summary>
    /// This interface should be implemented by vendors to
    /// make entity history working.
    /// </summary>
    public interface IEntityHistoryStore<in TEntityChangeSet, TPrimaryKey>
        where TEntityChangeSet : EntityChangeSet<TPrimaryKey> 
        where TPrimaryKey : struct, IEquatable<TPrimaryKey>
    { 
        /// <summary>
        /// Should save entity change set to a persistent store.
        /// </summary>
        /// <param name="entityChangeSet">Entity change set</param>
        Task SaveAsync(TEntityChangeSet entityChangeSet);
    }
}
