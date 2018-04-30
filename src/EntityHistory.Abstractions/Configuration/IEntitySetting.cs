﻿using System;
using System.Linq.Expressions;

namespace EntityHistory.Abstractions.Configuration
{
    /// <summary>
    /// The settings configuration for an Entity
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    public interface IEntitySetting<TEntity>
    {
        /// <summary>
        /// Ignore a column on the history trail indicating its entity property
        /// </summary>
        /// <typeparam name="TProp">The property type</typeparam>
        /// <param name="property">The property selector. Must be a simple property expression i.e. x -> x.PropertyName</param>
        IEntitySetting<TEntity> Ignore<TProp>(Expression<Func<TEntity, TProp>> property);

        /// <summary>
        /// Ignore a column on the history trail indicating its name
        /// </summary>
        /// <param name="propertyName">The entity's property name (case sensitive)</param>
        IEntitySetting<TEntity> Ignore(string propertyName);

        /// <summary>
        /// Overrides a column value to a constant on the history trail.
        /// </summary>
        /// <typeparam name="TProp">The property type</typeparam>
        /// <param name="property">The property selector. Must be a simple property expression i.e. x -> x.PropertyName</param>
        /// <param name="value">The constant value to store on the history trail</param>
        IEntitySetting<TEntity> Override<TProp>(Expression<Func<TEntity, TProp>> property, TProp value);

        /// <summary>
        /// Overrides a column value to a constant on the history trail.
        /// </summary>
        /// <param name="propertyName">The entity's property name (case sensitive)</param>
        /// <param name="value">The constant value to store on the history trail</param>
        IEntitySetting<TEntity> Override<TProp>(string propertyName, TProp value);

        /// <summary>
        /// Overrides a column value to a value given as a function of the current value.
        /// </summary>
        /// <param name="property">The property selector. Must be a simple property expression i.e. x -> x.PropertyName</param>
        /// <param name="format">A function of the current value that returns the value to override</param>
        IEntitySetting<TEntity> Format<TProp>(Expression<Func<TEntity, TProp>> property, Func<TProp, TProp> format);

        /// <summary>
        /// Overrides a column value to a value given as a function of the current value.
        /// </summary>
        /// <param name="propertyName">The entity's property name (case sensitive)</param>
        /// <param name="format">A function of the current value that returns the value to override</param>
        IEntitySetting<TEntity> Format<TProp>(string propertyName, Func<TProp, TProp> format);
    }
}
