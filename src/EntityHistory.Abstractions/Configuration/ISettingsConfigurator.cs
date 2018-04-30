using System;

namespace EntityHistory.Abstractions.Configuration
{
    public interface ISettingsConfigurator
    {
        /// <summary>
        /// Sets the configuration for a specific entity (table)
        /// </summary>
        /// <param name="config">The configuration.</param>
        ISettingsConfigurator ForEntity<TEntity>(Action<IEntitySetting<TEntity>> config);

        /// <summary>
        /// Include all property
        /// </summary>
        void ForEntity<TEntity>();

        /// <summary>
        /// Include all property
        /// </summary>
        void ForEntity(Type entityType);
    }
}
