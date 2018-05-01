using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EntityHistory.Abstractions.Configuration;
using EntityHistory.Configuration.Attributes;
using JetBrains.Annotations;

namespace EntityHistory.Configuration
{
    public static class GlobalConfigHelper
    {
        private static readonly Dictionary<Type, EntityConfigContainer> CurrentConfig = new Dictionary<Type, EntityConfigContainer>();

        [CanBeNull]
        public static EntityConfigContainer GetEntityConfigContainer<TEntity>()
        {
            return GetEntityConfigContainer(typeof(TEntity));
        }

        [CanBeNull]
        public static EntityConfigContainer GetEntityConfigContainer(Type entityType)
        {
            if (CurrentConfig.TryGetValue(entityType, out EntityConfigContainer result))
            {
                return result;
            }

            return null;
        }

        public static bool IsIncluded<TEntity>(bool defaultValue)
        {
            return IsIncluded(typeof(TEntity), defaultValue);
        }

        public static bool IsIncluded(Type entityType, bool defaultValue)
        {
            if (CurrentConfig.ContainsKey(entityType))
            {
                return true;
            }

            if (entityType.GetTypeInfo().IsDefined(typeof(HistoryIncludeAttribute), true))
            {
                return true;
            }

            var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (properties.Any(p => p?.IsDefined(typeof(HistoryIncludeAttribute)) ?? false))
            {
                return true;
            }

            return defaultValue;
        }

        public static bool IsIncluded<TEntity>(string propertyName, bool defaultValue)
        {
            return IsIncluded(typeof(TEntity), propertyName, defaultValue);
        }

        public static bool IsIncluded(Type entityType, string propertyName, bool defaultValue)
        {
            var container = GetEntityConfigContainer(entityType);

            if (container != null && container.IgnoredProperties.Contains(propertyName))
            {
                return false;
            }

            var propertyInfo = entityType.GetProperty(propertyName);
            if (propertyInfo != null && propertyInfo.IsDefined(typeof(HistoryIgnoreAttribute), true))
            {
                return false;
            }

            return defaultValue;
        }

        public static void Reset<TEntity>()
        {
            CurrentConfig.Remove(typeof(TEntity));
        }

        internal static void SetEntityConfig<TEntity>(Action<IEntityConfiguration<TEntity>> config)
        {
            var entitySettings = new EntityConfiguration<TEntity>();
            config.Invoke(entitySettings);

            var entityConfig = EnsureConfigForEntity<TEntity>();
            entityConfig.IgnoredProperties = entitySettings.IgnoredProperties;
            entityConfig.OverrideProperties = entitySettings.OverrideProperties;
            entityConfig.FormatProperties = entitySettings.FormatProperties;
        }

        internal static void SetEntityConfig(Type entityType)
        {
            EnsureConfigForEntity(entityType);
        }

        #region Private Methods

        private static EntityConfigContainer EnsureConfigForEntity<TEntity>()
        {
            return EnsureConfigForEntity(typeof(TEntity));
        }

        private static EntityConfigContainer EnsureConfigForEntity(Type entityType)
        {
            if (CurrentConfig.TryGetValue(entityType, out EntityConfigContainer value))
            {
                return value;
            }

            CurrentConfig[entityType] = new EntityConfigContainer();
            return CurrentConfig[entityType];
        }

        #endregion
    }
}
