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
            //var entity = entityEntry.Entity as IEntity<TKey>;
            //if (entity != null)
            //{
            //    return entity.Id.ToString();
            //}

            var primaryKey = entityEntry.Metadata.FindPrimaryKey();

            return string.Empty;
            //return entityAsObj
            //    .GetType().GetProperty("Id")?
            //    .GetValue(entityAsObj)?
            //    .ToJsonString();
        }
    }
}
