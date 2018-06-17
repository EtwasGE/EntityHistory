using System;
using EntityHistory.Abstractions.Configuration;

namespace EntityHistory.Configuration.FluentApi
{
    internal class Configurator : IConfigurator
    {
        public virtual IConfigurator ForEntity<TEntity>(Action<IEntityConfiguration<TEntity>> config)
        {
            HistoryConfiguration.SetEntityConfig(config);
            return this;
        }

        public virtual IConfigurator AllInclude<TEntity>()
        {
            HistoryConfiguration.SetEntityConfig(typeof(TEntity));
            return this;
        }

        public virtual IConfigurator AllInclude(Type entityType)
        {
            HistoryConfiguration.SetEntityConfig(entityType);
            return this;
        }

        public virtual IConfigurator ApplyConfiguration<TEntity>(IConfigurationModule<TEntity> module)
        {
            var action = new Action<IEntityConfiguration<TEntity>>(module.Configure);
            HistoryConfiguration.SetEntityConfig(action);
            return this;
        }
    }
}
