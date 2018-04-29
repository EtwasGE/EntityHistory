using System;
using EntityHistory.Core.Entities;
using EntityHistory.Core.Interfaces;
using Nito.AsyncEx;

namespace EntityHistory.Core.Extensions
{
    public static class EntityHistoryHelperExtensions
    {
        public static void Save<TEntityEntry, TEntityChangeSet, TPrimaryKey>(this IEntityHistoryHelper<TEntityEntry, TEntityChangeSet, TPrimaryKey> entityHistoryHelper, TEntityChangeSet changeSet)
            where TEntityChangeSet : EntityChangeSet<TPrimaryKey> 
            where TPrimaryKey : struct, IEquatable<TPrimaryKey>
        {
            AsyncContext.Run(() => entityHistoryHelper.SaveAsync(changeSet));
        }
    }
}
