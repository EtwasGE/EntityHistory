using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EntityHistory.Abstractions.Configuration;
using EntityHistory.Configuration.Attributes;
using EntityHistory.Configuration.FluentApi;
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

        public static bool IsIncluded<TEntity>()
        {
            return IsIncluded(typeof(TEntity));
        }
        
        public static bool IsIncluded(Type entityType)
        {
            if (entityType == null)
            {
                return false;
            }

            // check registration type with fluent API
            if (CurrentConfig.ContainsKey(entityType))
            {
                return true;
            }

            // check all base types
            if(IsIncluded(entityType.BaseType))
            {
                return true;
            }
            
            if (entityType.GetTypeInfo().IsDefined(typeof(HistoryIncludeAttribute), true))
            {
                return true;
            }

            var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (properties.Any(p => p.IsDefined(typeof(HistoryIgnoreAttribute)) || p.IsDefined(typeof(HistoryOverrideAttribute))))
            {
                return true;
            }

            return false;
        }

        public static bool IsIncluded<TEntity>(string propertyName)
        {
            return IsIncluded(typeof(TEntity), propertyName);
        }

        public static bool IsIncluded(Type entityType, string propertyName)
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

            return true;
        }

        public static bool IsOverridden<TEntity>(string propertyName, out object result)
        {
            return IsOverridden(typeof(TEntity), propertyName, out result);
        }

        public static bool IsOverridden(Type entityType, string propertyName, out object result)
        {
            if (CurrentConfig.TryGetValue(entityType, out EntityConfigContainer value))
            {
                return value.OverrideProperties.TryGetValue(propertyName, out result);
            }

            var propertyInfo = entityType.GetProperty(propertyName);
            if (propertyInfo != null && propertyInfo.IsDefined(typeof(HistoryOverrideAttribute), true))
            {
                result = propertyInfo.GetCustomAttribute<HistoryOverrideAttribute>()?.Value;
                return true;
            }

            result = null;
            return false;
        }

        public static bool IsFormated<TEntity>(string propertyName, out Func<object, object> result)
        {
            return IsFormated(typeof(TEntity), propertyName, out result);
        }

        public static bool IsFormated(Type entityType, string propertyName, out Func<object, object> result)
        {
            if (CurrentConfig.TryGetValue(entityType, out EntityConfigContainer value))
            {
                return value.FormatProperties.TryGetValue(propertyName, out result);
            }

            result = null;
            return false;
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
