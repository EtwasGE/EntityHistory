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
        private static readonly Dictionary<Type, EntitySettingsContainer> CurrentConfig = new Dictionary<Type, EntitySettingsContainer>();

        [CanBeNull]
        public static EntitySettingsContainer GetEntitySettingsContainer<TEntity>()
        {
            return GetEntitySettingsContainer(typeof(TEntity));
        }

        [CanBeNull]
        public static EntitySettingsContainer GetEntitySettingsContainer(Type entityType)
        {
            if (CurrentConfig.TryGetValue(entityType, out EntitySettingsContainer result))
            {
                return result;
            }

            return null;
        }

        public static bool IsIncluded<TEntity>(bool defaultValue = false)
        {
            return IsIncluded(typeof(TEntity), defaultValue);
        }

        public static bool IsIncluded(Type entityType, bool defaultValue = false)
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
            var container = GetEntitySettingsContainer(entityType);

            if (container != null)
            {
                if (container.IgnoredProperties.Contains(propertyName))
                {
                    return false;
                }

                if (!container.FormatProperties.ContainsKey(propertyName)
                    && !container.OverrideProperties.ContainsKey(propertyName))
                {
                    return false;
                }
            }

            var propertyInfo = entityType.GetProperty(propertyName);
            if (propertyInfo != null && propertyInfo.IsDefined(typeof(HistoryIgnoreAttribute), true))
            {
                return false;
            }

            var classType = propertyInfo?.DeclaringType;
            if (classType != null)
            {
                if (!classType.GetTypeInfo().IsDefined(typeof(HistoryIncludeAttribute), true) &&
                    !propertyInfo.IsDefined(typeof(HistoryIncludeAttribute), true))
                {
                    return false;
                }
            }

            return defaultValue;
        }

        public static void Reset<TEntity>()
        {
            CurrentConfig.Remove(typeof(TEntity));
        }

        internal static void SetEntitySetting<TEntity>(Action<IEntitySetting<TEntity>> config)
        {
            var entitySettings = new EntitySetting<TEntity>();
            config.Invoke(entitySettings);

            var entityConfig = EnsureConfigForEntity<TEntity>();
            entityConfig.IgnoredProperties = entitySettings.IgnoredProperties;
            entityConfig.OverrideProperties = entitySettings.OverrideProperties;
            entityConfig.FormatProperties = entitySettings.FormatProperties;
        }

        internal static void SetEntitySetting(Type entityType)
        {
            EnsureConfigForEntity(entityType);
        }

        #region Private Methods

        private static EntitySettingsContainer EnsureConfigForEntity<TEntity>()
        {
            return EnsureConfigForEntity(typeof(TEntity));
        }

        private static EntitySettingsContainer EnsureConfigForEntity(Type entityType)
        {
            if (CurrentConfig.TryGetValue(entityType, out EntitySettingsContainer value))
            {
                return value;
            }

            CurrentConfig[entityType] = new EntitySettingsContainer();
            return CurrentConfig[entityType];
        }

        #endregion
    }
}
