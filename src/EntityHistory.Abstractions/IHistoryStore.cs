﻿using System.Threading.Tasks;

namespace EntityHistory.Abstractions
{
    /// <summary>
    /// This interface should be implemented by vendors to
    /// make entity history working.
    /// </summary>
    public interface IHistoryStore
    { 
        /// <summary>
        /// Should save entity change set to a persistent store.
        /// </summary>
        /// <param name="entityChangeSet">Entity change set</param>
        Task SaveAsync<TEntityChangeSet>(TEntityChangeSet entityChangeSet) where TEntityChangeSet : class;
    }
}