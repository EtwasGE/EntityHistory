using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EntityHistory.EntityFrameworkCore.Common.Extensions
{
    public static class EntityEntryExtensions
    {
        public static bool IsCreated(this EntityEntry entityEntry)
        {
            return entityEntry.State == EntityState.Added;
        }

        public static bool IsDeleted(this EntityEntry entityEntry)
        {
            return entityEntry.State == EntityState.Deleted;
        }

        public static string GetPrimaryKeyValue(this EntityEntry entityEntry)
        {
            var primaryKey = entityEntry.Metadata.FindPrimaryKey();
            var primaryKeyProperty = primaryKey?.Properties.FirstOrDefault();

            if (primaryKeyProperty != null)
            {
                var propertyEntry = entityEntry.Property(primaryKeyProperty.Name);

                if (propertyEntry != null)
                {
                    return propertyEntry.CurrentValue.ToString(); //ToJsonString()
                }
            }

            return string.Empty;
        }
    }
}
