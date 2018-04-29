using EntityHistory.Core.Interfaces;
using Nito.AsyncEx;

namespace EntityHistory.Core.Extensions
{
    public static class EntityHistoryHelperExtensions
    {
        public static void Save<TEntityEntry>(this IEntityHistoryHelper<TEntityEntry> entityHistoryHelper, IEntityChangeSet changeSet)
        {
            AsyncContext.Run(() => entityHistoryHelper.SaveAsync(changeSet));
        }
    }
}
