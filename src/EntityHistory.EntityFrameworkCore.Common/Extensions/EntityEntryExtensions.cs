using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EntityHistory.EntityFrameworkCore.Common.Extensions
{
    public static class EntityEntryExtensions
    {
        public static bool IsCreated([NotNull] this EntityEntry entityEntry)
        {
            return entityEntry.State == EntityState.Added;
        }

        public static bool IsDeleted([NotNull] this EntityEntry entityEntry)
        {
            return entityEntry.State == EntityState.Deleted;
        }

        public static string GetPrimaryKeyValue([NotNull] this EntityEntry entityEntry)
        {
            var primaryKey = entityEntry.Metadata.FindPrimaryKey();
            var primaryKeyProperty = primaryKey?.Properties.FirstOrDefault();

            if (primaryKeyProperty != null)
            {
                var propertyEntry = entityEntry.Property(primaryKeyProperty.Name);

                if (propertyEntry != null)
                {
                    return propertyEntry.CurrentValue.ToString();
                }
            }

            return string.Empty;
        }
    }
}
