using System;
using System.Collections.Generic;
using EntityHistory.Abstractions.Configuration;
using EntityHistory.Configuration.FluentApi;
using EntityHistory.Core.Entities;

namespace EntityHistory.Configuration
{
    public class EntityHistoryConfiguration : IEntityHistoryConfiguration
    {
        public bool IsEnabled { get; protected set; }

        public bool IsEnabledForAnonymousUsers { get; protected set; }

        public List<Type> IgnoredTypes { get; protected set; }

        public EntityHistoryConfiguration()
        {
            Initial();
        }

        public void Initial()
        {
            IsEnabled = true;
            IsEnabledForAnonymousUsers = false;

            IgnoredTypes = new List<Type>
            {
                typeof(EntityChangeSet<>),
                typeof(EntityChange),
                typeof(EntityPropertyChange)
            };

            OnRegistration(new EntitiesConfigurator());
        }

        protected virtual void OnRegistration(IEntitiesConfigurator config)
        {
        }
    }
}
