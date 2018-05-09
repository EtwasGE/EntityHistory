﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityHistory.Abstractions;
using EntityHistory.Abstractions.Auditing;
using EntityHistory.Abstractions.Configuration;
using EntityHistory.Abstractions.Session;
using EntityHistory.Core.Auditing;
using EntityHistory.Core.Entities;
using EntityHistory.Core.Extensions;
using EntityHistory.Core.Session;
using JetBrains.Annotations;

namespace EntityHistory.Core.History
{
    public abstract class HistoryHelperBase<TEntityEntry, TEntityChangeSet, TUserKey>
        : IHistoryHelper<TEntityEntry, TEntityChangeSet>
        where TEntityChangeSet : EntityChangeSet<TUserKey>, new()
        where TUserKey : struct, IEquatable<TUserKey>
    {
        public ISession<TUserKey> Session { protected get; set; }
        public IClientInfoProvider ClientInfoProvider { protected get; set; }
        public IHistoryStore HistoryStore { protected get; set; }

        protected IHistoryConfiguration Configuration { get; }

        protected HistoryHelperBase(IHistoryConfiguration configuration)
        {
            Session = NullSession<TUserKey>.Instance;
            ClientInfoProvider = NullClientInfoProvider.Instance;
            HistoryStore = NullHistoryStore.Instance;

            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected abstract bool ShouldSaveEntityHistory(TEntityEntry entityEntry);
        protected abstract EntityChange GetEntityChange(TEntityEntry entityEntry);
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
        
        public virtual async Task UpdateAndSaveAsync(TEntityChangeSet changeSet)
        {
            if (!IsEntityHistoryEnabled)
            {
                return;
            }

            if (changeSet.EntityChanges.Count == 0 || changeSet.EntityChanges.All(x=> x.PropertyChanges.Count == 0))
            {
                return;
            }

            UpdateChangeSet(changeSet);
            await HistoryStore.SaveAsync(changeSet);
        }
    }
}