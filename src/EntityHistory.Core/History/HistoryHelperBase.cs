using System;
using System.Collections.Generic;
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
    public abstract class HistoryHelperBase<TEntityEntry, TEntityChangeSet, TUserKey> : IHistoryHelper<TEntityEntry, TEntityChangeSet>
        where TEntityChangeSet : EntityChangeSet<TUserKey>, new()
        where TUserKey : struct, IEquatable<TUserKey>

    {
        public ISession<TUserKey> Session { protected get; set; }
        public IClientInfoProvider ClientInfoProvider { protected get; set; }
       
        protected HistoryHelperBase()
        {
            Session = NullSession<TUserKey>.Instance;
            ClientInfoProvider = NullClientInfoProvider.Instance;
        }

        protected abstract bool ShouldSaveEntityHistory(TEntityEntry entityEntry);
        protected abstract EntityChange GetEntityChange(TEntityEntry entityEntry);
        protected abstract void UpdateChangeSet(TEntityChangeSet entityChangeSet);

        protected abstract bool IsEntityHistoryEnabled { get; }

        [CanBeNull]
        public virtual TEntityChangeSet GetEntityChangeSet(ICollection<TEntityEntry> entityEntries)
        {
            if (!IsEntityHistoryEnabled)
            {
                return null;
            }

            var changeSet = new TEntityChangeSet
            {
                BrowserInfo = ClientInfoProvider.BrowserInfo.TruncateWithPostfix(EntityChangeSet<TUserKey>.MaxBrowserInfoLength),
                ClientIpAddress = ClientInfoProvider.ClientIpAddress.TruncateWithPostfix(EntityChangeSet<TUserKey>.MaxClientIpAddressLength),
                ClientName = ClientInfoProvider.ComputerName.TruncateWithPostfix(EntityChangeSet<TUserKey>.MaxClientNameLength),
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
        
        public virtual void UpdateEntityChangeSet(TEntityChangeSet changeSet)
        {
            if (!IsEntityHistoryEnabled)
            {
                return;
            }
            
            UpdateChangeSet(changeSet);
        }
    }
}
