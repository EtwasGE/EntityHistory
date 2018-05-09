using EntityHistory.Abstractions;
using Nito.AsyncEx;

namespace EntityHistory.Core.Extensions
{
    public static class HistoryHelperExtensions
    {
        public static void UpdateAndSave<TEntityEntry, TEntityChangeSet>(this IHistoryHelper<TEntityEntry, TEntityChangeSet> historyHelper, TEntityChangeSet changeSet)
        {
            AsyncContext.Run(() => historyHelper.UpdateAndSaveAsync(changeSet));
        }
    }
}
