using System;

namespace EntityHistory.Abstractions.Configuration
{
    public interface IConfigurator
    {
        /// <summary>
        /// Sets the configuration for a specific entity (table)
        /// </summary>
        /// <param name="config">The configuration.</param>
        IConfigurator ForEntity<TEntity>(Action<IEntityConfiguration<TEntity>> config);

        /// <summary>
        /// Include all property
        /// </summary>
        IConfigurator AllInclude<TEntity>();

        /// <summary>
        /// Include all property
        /// </summary>
        IConfigurator AllInclude(Type entityType);

        /// <summary>
        /// 
        /// </summary>
        IConfigurator ApplyConfiguration<TEntity>(IConfigurationModule<TEntity> module);
    }
}
