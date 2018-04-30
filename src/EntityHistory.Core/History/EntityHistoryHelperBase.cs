using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityHistory.Abstractions;
using EntityHistory.Abstractions.Auditing;
using EntityHistory.Abstractions.Session;
using EntityHistory.Core.Auditing;
using EntityHistory.Core.Entities;
using EntityHistory.Core.Extensions;
using EntityHistory.Core.Session;
using JetBrains.Annotations;

namespace EntityHistory.Core.History
{
    public abstract class EntityHistoryHelperBase<TEntityEntry, TEntityChangeSet, TEntityChange, TKey>
        : IEntityHistoryHelper<TEntityEntry, TEntityChangeSet>
        where TEntityChangeSet : EntityChangeSet<TKey>, new()
        where TEntityChange : EntityChange<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        protected ISession<TKey> Session { get; set; }
        protected IClientInfoProvider ClientInfoProvider { get; set; }
        protected IEntityHistoryStore EntityHistoryStore { get; set; }

        protected IEntityHistoryConfiguration Configuration { get; }

        protected EntityHistoryHelperBase(IEntityHistoryConfiguration configuration)
        {
            Session = NullSession<TKey>.Instance;
            ClientInfoProvider = NullClientInfoProvider.Instance;
            EntityHistoryStore = NullEntityHistoryStore.Instance;

            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected abstract bool ShouldSaveEntityHistory(TEntityEntry entityEntry);
        protected abstract TEntityChange GetEntityChange(TEntityEntry entityEntry);
        protected abstract void UpdateChangeSet(TEntityChangeSet entityChangeSet);

        protected bool IsEntityHistoryEnabled
        {
            get
            {
                if (!Configuration.IsEnabled)
                {
                    return false;
                }

                if (!Configuration.IsEnabledForAnonymousUsers && Session?.UserId == null)
                {
                    return false;
                }

                return true;
            }
        }

        [CanBeNull]
        public virtual TEntityChangeSet GetEntityChangeSet(ICollection<TEntityEntry> entityEntries)
        {
            if (!IsEntityHistoryEnabled)
            {
                return null;
            }

            var changeSet = new TEntityChangeSet
            {
                BrowserInfo = ClientInfoProvider.BrowserInfo.TruncateWithPostfix(EntityChangeSet<TKey>.MaxBrowserInfoLength),
                ClientIpAddress = ClientInfoProvider.ClientIpAddress.TruncateWithPostfix(EntityChangeSet<TKey>.MaxClientIpAddressLength),
                ClientName = ClientInfoProvider.ComputerName.TruncateWithPostfix(EntityChangeSet<TKey>.MaxClientNameLength),
                UserId = Session.UserId
            };

            foreach (var entry in entityEntries)
            {
                if (!ShouldSaveEntityHistory(entry))
                {
                    continue;
                }

                var entityChange = GetEntityChange(entry);
                if (entityChange != null)
                {
                    changeSet.EntityChanges.Add(entityChange);
                } 
            }

            return changeSet;
        }
        
        public virtual async Task UpdateAndSaveAsync(TEntityChangeSet changeSet)
        {
            if (!IsEntityHistoryEnabled)
            {
                return;
            }

            if (changeSet.EntityChanges.Count == 0)
            {
                return;
            }

            UpdateChangeSet(changeSet);
            await EntityHistoryStore.SaveAsync(changeSet);
        }
    }
}
