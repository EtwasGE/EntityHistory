using System;
using System.Collections.Generic;

namespace EntityHistory.Abstractions.Configuration
{
    /// <summary>
    /// Used to configure entity history.
    /// </summary>
    public interface IEntityHistoryConfiguration
    {
        /// <summary>
        /// Used to enable/disable entity history system.
        /// Default: true. Set false to completely disable it.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Set true to enable saving entity history if current user is not logged in.
        /// Default: false.
        /// </summary>
        bool IsEnabledForAnonymousUsers { get; set; }
        
        /// <summary>
        /// Ignored types for serialization on entity history tracking.
        /// </summary>
        List<Type> IgnoredTypes { get; }

        /// <summary>
        /// Sets the configuration for a specific entity (table)
        /// </summary>
        void Settings(ISettingsConfigurator config);
    }
}