using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EntityHistory.EntityFrameworkCore.Common
{
    //public static class EntityKeyHelper
    //{
    //    /// <summary>
    //    /// Gets the primary key values for an entity
    //    /// </summary>
    //    public static Dictionary<string, object> GetPrimaryKey(EntityEntry entry)
    //    {
    //        var result = new Dictionary<string, object>();
    //        foreach (var prop in entry.Properties.Where(p => p.Metadata.IsPrimaryKey()))
    //        {
    //            result.Add(GetColumnName(prop.Metadata), prop.CurrentValue);
    //        }
    //        return result;
    //    }

    //    /// <summary>
    //    /// Gets the foreign key values for an entity
    //    /// </summary>
    //    public static Dictionary<string, object> GetForeignKeys(EntityEntry entry)
    //    {
    //        var result = new Dictionary<string, object>();
    //        var foreignKeys = entry.Metadata.GetForeignKeys();
    //        if (foreignKeys != null)
    //        {
    //            foreach (var fk in foreignKeys)
    //            {
    //                foreach (var prop in fk.Properties)
    //                {
    //                    result.Add(GetColumnName(prop), entry.Property(prop.Name).CurrentValue);
    //                }
    //            }
    //        }
    //        return result;
    //    }
    //}
}
