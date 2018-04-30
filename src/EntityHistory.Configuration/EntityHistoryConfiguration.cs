using System;
using System.Collections.Generic;
using EntityHistory.Abstractions.Configuration;
using EntityHistory.Core.Entities;

namespace EntityHistory.Configuration
{
    public class EntityHistoryConfiguration : IEntityHistoryConfiguration
    {
        public bool IsEnabled { get; set; }

        public bool IsEnabledForAnonymousUsers { get; set; }

        public List<Type> IgnoredTypes { get; }

        public EntityHistoryConfiguration()
        {
            IsEnabled = true;
            IsEnabledForAnonymousUsers = false;

            IgnoredTypes = new List<Type>
            {
                typeof(EntityChangeSet<>),
                typeof(EntityChange<>),
                typeof(EntityPropertyChange<>)
            };
        }

        public virtual void Settings(ISettingsConfigurator config)
        {
            config.ForEntity<EntityChange<long>>();
        }
    }
}
