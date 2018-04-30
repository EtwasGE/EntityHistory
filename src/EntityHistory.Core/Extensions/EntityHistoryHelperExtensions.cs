using EntityHistory.Abstractions;
using Nito.AsyncEx;

namespace EntityHistory.Core.Extensions
{
    public static class EntityHistoryHelperExtensions
    {
        public static void UpdateAndSave<TEntityEntry, TEntityChangeSet>(this IEntityHistoryHelper<TEntityEntry, TEntityChangeSet> entityHistoryHelper, TEntityChangeSet changeSet)
        {
            AsyncContext.Run(() => entityHistoryHelper.UpdateAndSaveAsync(changeSet));
        }
    }
}
