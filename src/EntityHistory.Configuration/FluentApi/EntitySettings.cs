using System;
using System.Collections.Generic;

namespace EntityHistory.Configuration.FluentApi
{
    internal class EntitySettings
    {
        /// <summary>
        /// To indicate the entity's properties (columns) to be ignored on the entity history. Key: property name.
        /// </summary>
        public List<string> IgnoredProperties = new List<string>();

        /// <summary>
        /// To indicate constant values to override properties on the entity history. Key: property name, Value: constant value.
        /// </summary>
        public Dictionary<string, object> OverrideProperties = new Dictionary<string, object>();

        /// <summary>
        /// To indicate replacement functions for the property's values on the entity history. Key: property name, Value: function of the actual value.
        /// </summary>
        public Dictionary<string, Func<object, object>> FormatProperties = new Dictionary<string, Func<object, object>>();
    }
}
