﻿using System;
using EntityHistory.Abstractions.Configuration;

namespace EntityHistory.Configuration.FluentApi
{
    public class EntitiesConfigurator : IEntitiesConfigurator
    {
        public IEntitiesConfigurator ForEntity<TEntity>(Action<IEntityConfiguration<TEntity>> config)
        {
            GlobalConfigHelper.SetEntityConfig(config);
            return this;
        }

        public void AllInclude<TEntity>()
        {
            GlobalConfigHelper.SetEntityConfig(typeof(TEntity));
        }

        public void AllInclude(Type entityType)
        {
            GlobalConfigHelper.SetEntityConfig(entityType);
        }

        public IEntitiesConfigurator ApplyConfiguration<TEntity>(IConfigurationModule<TEntity> module)
        {
            var action = new Action<IEntityConfiguration<TEntity>>(module.Configure);
            GlobalConfigHelper.SetEntityConfig(action);
            return this;
        }
    }
}