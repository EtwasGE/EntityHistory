using System;
using System.Linq;
using System.Threading.Tasks;
using EntityHistory.Core.Entities;
using EntityHistory.Core.Extensions;
using EntityHistory.EntityFrameworkCore.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EntityHistory.EntityFrameworkCore.Common
{
    public class HistoryDbContextHelper : HistoryDbContextHelper<long>
    {
        public HistoryDbContextHelper(IHistoryHelper<EntityChangeSet<long>> historyHelper) 
            : base(historyHelper)
        {
        }
    }

    public class HistoryDbContextHelper<TUserKey> : HistoryDbContextHelper<EntityChangeSet<TUserKey>, TUserKey>
        where TUserKey : struct, IEquatable<TUserKey>
    {
        public HistoryDbContextHelper(IHistoryHelper<EntityChangeSet<TUserKey>> historyHelper) 
            : base(historyHelper)
        {
        }
    }

    public class HistoryDbContextHelper<TEntityChangeSet, TUserKey> : IHistoryDbContextHelper
        where TEntityChangeSet : EntityChangeSet<TUserKey>
        where TUserKey : struct, IEquatable<TUserKey>
    {
        private readonly IHistoryHelper<TEntityChangeSet> _historyHelper;

        public HistoryDbContextHelper(IHistoryHelper<TEntityChangeSet> historyHelper)
        {
            _historyHelper = historyHelper ?? throw new ArgumentNullException(nameof(historyHelper));
        }

        /// <summary>
        /// Saves the changes asynchronously.
        /// </summary>
        public virtual async Task<int> SaveChangesAsync(DbContext context, Func<Task<int>> baseSaveChanges)
        {
            var changeSet = _historyHelper.GetEntityChangeSet(context.ChangeTracker.Entries().ToList());

            var result = await baseSaveChanges.Invoke();

            if (changeSet != null)
            {
                await _historyHelper.UpdateAndSaveAsync(changeSet);
            }

            return result;
        }

        /// <summary>
        /// Saves the changes synchronously.
        /// </summary>
        public virtual int SaveChanges(DbContext context, Func<int> baseSaveChanges)
        {
            var changeSet = _historyHelper.GetEntityChangeSet(context.ChangeTracker.Entries().ToList());

            var result = baseSaveChanges.Invoke();

            if (changeSet != null)
            {
                _historyHelper.UpdateAndSave(changeSet);
            }

            return result;
        }
    }
}
