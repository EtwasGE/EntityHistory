using System;
using EntityHistory.Configuration;
using EntityHistory.Core.Entities;
using EntityHistory.Core.Extensions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EntityHistory.EntityFrameworkCore.Common.Extensions
{
    internal static class PropertyEntryExtensions
    {
        [CanBeNull]
        public static string GetCurrentValue(this PropertyEntry propertyEntry)
        {
            var entityType = propertyEntry.EntityEntry.Metadata.ClrType;
            var propertyName = propertyEntry.Metadata.Name;

            if (HistoryConfiguration.IsOverridden(entityType, propertyName, out object overrideValue))
            {
                return overrideValue?.ToString().TruncateWithPostfix(EntityPropertyChange.MaxValueLength);
            }

            if (HistoryConfiguration.IsFormated(entityType, propertyName, out Func<object, object> formatedFunc))
            {
                return formatedFunc.Invoke(propertyEntry.CurrentValue)?.ToString().TruncateWithPostfix(EntityPropertyChange.MaxValueLength);
            }


            return propertyEntry.CurrentValue?.ToString().TruncateWithPostfix(EntityPropertyChange.MaxValueLength);
        }

        [CanBeNull]
        public static string GetOriginalValue(this PropertyEntry propertyEntry)
        {
            var entityType = propertyEntry.EntityEntry.Metadata.ClrType;
            var propertyName = propertyEntry.Metadata.Name;

            if (HistoryConfiguration.IsOverridden(entityType, propertyName, out object overrideValue))
            {
                return overrideValue?.ToString().TruncateWithPostfix(EntityPropertyChange.MaxValueLength);
            }

            return propertyEntry.OriginalValue?.ToString().TruncateWithPostfix(EntityPropertyChange.MaxValueLength);
        }
    }
}
