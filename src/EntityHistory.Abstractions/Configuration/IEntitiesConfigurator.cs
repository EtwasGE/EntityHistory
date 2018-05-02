using System;

namespace EntityHistory.Abstractions.Configuration
{
    public interface IEntitiesConfigurator
    {
        /// <summary>
        /// Sets the configuration for a specific entity (table)
        /// </summary>
        /// <param name="config">The configuration.</param>
        IEntitiesConfigurator ForEntity<TEntity>(Action<IEntityConfiguration<TEntity>> config);

        /// <summary>
        /// Include all property
        /// </summary>
        void AllInclude<TEntity>();

        /// <summary>
        /// Include all property
        /// </summary>
        void AllInclude(Type entityType);

        /// <summary>
        /// 
        /// </summary>
        IEntitiesConfigurator ApplyConfiguration<TEntity>(IConfigurationModule<TEntity> module);
    }
}
