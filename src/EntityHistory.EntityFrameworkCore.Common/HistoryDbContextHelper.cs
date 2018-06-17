using System;
using System.Linq;
using System.Threading.Tasks;
using EntityHistory.Abstractions;
using EntityHistory.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EntityHistory.EntityFrameworkCore.Common
{
    public class HistoryDbContextHelper<TEntityChangeSet, TUserKey> : IHistoryDbContextHelper<DbContext>
        where TEntityChangeSet : EntityChangeSet<TUserKey>
        where TUserKey : struct, IEquatable<TUserKey>
    {
        private readonly IHistoryHelper<EntityEntry, TEntityChangeSet> _historyHelper;

        public HistoryDbContextHelper(IHistoryHelper<EntityEntry, TEntityChangeSet> historyHelper)
        {
            _historyHelper = historyHelper ?? throw new ArgumentNullException(nameof(historyHelper));
        }

        /// <summary>
        /// Saves the changes asynchronously.
        /// </summary>
        public virtual async Task<int> SaveChangesAsync(DbContext context, Func<Task<int>> baseSaveChanges)
        {
            var changeSet = _historyHelper.GetEntityChangeSet(context.ChangeTracker.Entries().ToList());

            var result = await baseSaveChanges();

            if (changeSet != null 
                && changeSet.EntityChanges.Count != 0 
                && changeSet.EntityChanges.Any(x => x.PropertyChanges.Count != 0))
            {
                _historyHelper.UpdateEntityChangeSet(changeSet);

                await context.Set<TEntityChangeSet>().AddAsync(changeSet);
                await baseSaveChanges();
            }

            return result;
        }

        /// <summary>
        /// Saves the changes synchronously.
        /// </summary>
        public virtual int SaveChanges(DbContext context, Func<int> baseSaveChanges)
        {
            var changeSet = _historyHelper.GetEntityChangeSet(context.ChangeTracker.Entries().ToList());

            var result = baseSaveChanges();

            if (changeSet != null 
                && changeSet.EntityChanges.Count != 0 
                && changeSet.EntityChanges.Any(x => x.PropertyChanges.Count != 0))
            {
                _historyHelper.UpdateEntityChangeSet(changeSet);

                context.Set<TEntityChangeSet>().Add(changeSet);
                baseSaveChanges();
            }
            
            return result;
        }
    }
}
