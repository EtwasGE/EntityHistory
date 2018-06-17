using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EntityHistory.Abstractions.Configuration;
using EntityHistory.Configuration.Attributes;
using EntityHistory.Configuration.FluentApi;
using EntityHistory.Core.Entities;
using JetBrains.Annotations;

namespace EntityHistory.Configuration
{
    public static class HistoryConfiguration
    {
        private static readonly Dictionary<Type, EntitySettings> CurrentConfig = new Dictionary<Type, EntitySettings>();

        /// <summary>
        /// Used to enable/disable entity history system.
        /// Default: true. Set false to completely disable it.
        /// </summary>
        public static bool IsEnabled { get; set; }

        /// <summary>
        /// Set true to enable saving entity history if current user is not logged in.
        /// Default: false.
        /// </summary>
        public static bool IsEnabledForAnonymousUsers { get; set; }

        /// <summary>
        /// Ignored types for serialization on entity history tracking.
        /// </summary>
        internal static List<Type> IgnoredTypes { get; set; }

        static HistoryConfiguration()
        {
            IsEnabled = true;
            IsEnabledForAnonymousUsers = false;

            IgnoredTypes = new List<Type>
            {
                typeof(EntityChangeSet<>),
                typeof(EntityChange),
                typeof(EntityPropertyChange)
            };
        }

        /// <summary>
        /// Configure entity history settings by using a Fluent Configuration API.
        /// </summary>
        public static IConfigurator Setup()
        {
            return new Configurator();
        }

        public static void Reset<TEntity>()
        {
            CurrentConfig.Remove(typeof(TEntity));
        }

        #region Internal Methods

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

        [CanBeNull]
        internal static EntitySettings GetEntitySettings<TEntity>()
        {
            return GetEntitySettings(typeof(TEntity));
        }

        [CanBeNull]
        internal static EntitySettings GetEntitySettings(Type entityType)
        {
            if (CurrentConfig.TryGetValue(entityType, out EntitySettings result))
            {
                return result;
            }

            return null;
        }

        internal static bool IsIncluded<TEntity>()
        {
            return IsIncluded(typeof(TEntity));
        }

        internal static bool IsIncluded(Type entityType)
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
            if (IsIncluded(entityType.BaseType))
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

        internal static bool IsIncluded<TEntity>(string propertyName)
        {
            return IsIncluded(typeof(TEntity), propertyName);
        }

        internal static bool IsIncluded(Type entityType, string propertyName)
        {
            var settings = GetEntitySettings(entityType);

            if (settings != null && settings.IgnoredProperties.Contains(propertyName))
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

        internal static bool IsOverridden<TEntity>(string propertyName, out object result)
        {
            return IsOverridden(typeof(TEntity), propertyName, out result);
        }

        internal static bool IsOverridden(Type entityType, string propertyName, out object result)
        {
            if (CurrentConfig.TryGetValue(entityType, out EntitySettings value))
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

        internal static bool IsFormated<TEntity>(string propertyName, out Func<object, object> result)
        {
            return IsFormated(typeof(TEntity), propertyName, out result);
        }

        internal static bool IsFormated(Type entityType, string propertyName, out Func<object, object> result)
        {
            if (CurrentConfig.TryGetValue(entityType, out EntitySettings value))
            {
                return value.FormatProperties.TryGetValue(propertyName, out result);
            }

            result = null;
            return false;
        }
        
        #endregion

        #region Private Methods

        private static EntitySettings EnsureConfigForEntity<TEntity>()
        {
            return EnsureConfigForEntity(typeof(TEntity));
        }

        private static EntitySettings EnsureConfigForEntity(Type entityType)
        {
            if (CurrentConfig.TryGetValue(entityType, out EntitySettings value))
            {
                return value;
            }

            CurrentConfig[entityType] = new EntitySettings();
            return CurrentConfig[entityType];
        }

        #endregion
    }
}
