using System;
using EntityHistory.Abstractions.Configuration;

namespace EntityHistory.Configuration
{
    public class SettingsConfigurator : ISettingsConfigurator
    {
        public ISettingsConfigurator ForEntity<TEntity>(Action<IEntitySetting<TEntity>> config)
        {
            GlobalConfigHelper.SetEntitySetting(config);
            return this;
        }

        public void ForEntity<TEntity>()
        {
            GlobalConfigHelper.SetEntitySetting(typeof(TEntity));
        }

        public void ForEntity(Type entityType)
        {
            GlobalConfigHelper.SetEntitySetting(entityType);
        }
    }
}
