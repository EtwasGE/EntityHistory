using System;
using System.Collections.Generic;
using System.Linq;
using EntityHistory.Abstractions;
using EntityHistory.Core;
using EntityHistory.Core.Entities;
using EntityHistory.Core.Extensions;
using EntityHistory.Core.History;
using EntityHistory.EntityFrameworkCore.Common.Extensions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EntityHistory.EntityFrameworkCore.Common
{
    public class EntityHistoryHelper<TEntityChangeSet, TEntityChange, TEntityPropertyChange, TKey>
        : EntityHistoryHelperBase<EntityEntry, TEntityChangeSet, TEntityChange, TKey>
        where TEntityChangeSet : EntityChangeSet<TKey>, new()
        where TEntityChange : EntityChange<TKey>, new()
        where TEntityPropertyChange : EntityPropertyChange<TKey>, new()
        where TKey : struct, IEquatable<TKey>
    {
        public EntityHistoryHelper(IEntityHistoryConfiguration configuration)
            : base(configuration)
        {
        }

        protected override bool ShouldSaveEntityHistory(EntityEntry entityEntry)
        {
            //if (entityEntry.State == EntityState.Detached ||
            //    entityEntry.State == EntityState.Unchanged)
            //{
            //    return false;
            //}

            //if (_configuration.IgnoredTypes.Any(t => t.IsInstanceOfType(entityEntry.Entity)))
            //{
            //    return false;
            //}

            //var entityType = entityEntry.Entity.GetType();
            //if (!EntityHelper.IsEntity(entityType))
            //{
            //    return false;
            //}

            //if (!entityType.IsPublic)
            //{
            //    return false;
            //}

            //if (entityType.GetTypeInfo().IsDefined(typeof(AuditedAttribute), true))
            //{
            //    return true;
            //}

            //if (entityType.GetTypeInfo().IsDefined(typeof(DisableAuditingAttribute), true))
            //{
            //    return false;
            //}

            //if (_configuration.Selectors.Any(selector => selector.Predicate(entityType)))
            //{
            //    return true;
            //}

            //var properties = entityEntry.Metadata.GetProperties();
            //if (properties.Any(p => p.PropertyInfo?.IsDefined(typeof(AuditedAttribute)) ?? false))
            //{
            //    return true;
            //}

            //return defaultValue;

            return true;
        }

        protected virtual bool ShouldSavePropertyHistory(PropertyEntry propertyEntry, bool defaultValue)
        {
            //if (propertyEntry.Metadata.Name == "Id")
            //{
            //    return false;
            //}

            //var propertyInfo = propertyEntry.Metadata.PropertyInfo;
            //if (propertyInfo.IsDefined(typeof(DisableAuditingAttribute), true))
            //{
            //    return false;
            //}

            //var classType = propertyInfo.DeclaringType;
            //if (classType != null)
            //{
            //    if (classType.GetTypeInfo().IsDefined(typeof(DisableAuditingAttribute), true) &&
            //        !propertyInfo.IsDefined(typeof(AuditedAttribute), true))
            //    {
            //        return false;
            //    }
            //}

            //if (propertyEntry.IsModified)
            //{
            //    return true;
            //}

            //return defaultValue;

            return true;
        }

        [CanBeNull]
        protected override TEntityChange GetEntityChange(EntityEntry entityEntry)
        {
            var entity = entityEntry.Entity;

            EntityChangeType changeType;
            switch (entityEntry.State)
            {
                case EntityState.Added:
                    changeType = EntityChangeType.Created;
                    break;
                case EntityState.Deleted:
                    changeType = EntityChangeType.Deleted;
                    break;
                case EntityState.Modified:
                    changeType = entityEntry.IsDeleted() ? EntityChangeType.Deleted : EntityChangeType.Updated;
                    break;
                case EntityState.Detached:
                case EntityState.Unchanged:
                default:
                    return null;
            }

            var entityId = entityEntry.GetPrimaryKeyValue();
            if (entityId == null && changeType != EntityChangeType.Created)
            {
                return null;
            }

            var entityType = entity.GetType();

            var entityChange = new TEntityChange
            {
                ChangeType = changeType,
                EntityEntry = entityEntry,
                EntityId = entityId,
                EntityTypeFullName = entityType.FullName,
                PropertyChanges = GetPropertyChanges(entityEntry).ToArray()
            };

            return entityChange;
        }

        protected virtual IEnumerable<TEntityPropertyChange> GetPropertyChanges(EntityEntry entityEntry)
        {
            var propertyChanges = new List<TEntityPropertyChange>();
            var properties = entityEntry.Metadata.GetProperties();
            var isCreated = entityEntry.IsCreated();
            var isDeleted = entityEntry.IsDeleted();

            foreach (var property in properties)
            {
                var propertyEntry = entityEntry.Property(property.Name);
                if (ShouldSavePropertyHistory(propertyEntry, isCreated || isDeleted))
                {
                    propertyChanges.Add(new TEntityPropertyChange
                    {
                        NewValue = isDeleted ? null : propertyEntry.CurrentValue.ToJsonString().TruncateWithPostfix(EntityPropertyChange<TKey>.MaxValueLength),
                        OriginalValue = isCreated ? null : propertyEntry.OriginalValue.ToJsonString().TruncateWithPostfix(EntityPropertyChange<TKey>.MaxValueLength),
                        PropertyName = property.Name,
                        PropertyTypeFullName = property.ClrType.FullName
                    });
                }
            }

            return propertyChanges;
        }

        protected override void UpdateChangeSet(TEntityChangeSet entityChangeSet)
        {
            entityChangeSet.CreationTime = DateTime.Now;

            foreach (var entityChange in entityChangeSet.EntityChanges)
            {
                /* Update entity id */

                var entityEntry = entityChange.EntityEntry.As<EntityEntry>();
                entityChange.EntityId = entityEntry.GetPrimaryKeyValue();

                /* Update foreign keys */

                var foreignKeys = entityEntry.Metadata.GetForeignKeys();

                foreach (var foreignKey in foreignKeys)
                {
                    foreach (var property in foreignKey.Properties)
                    {
                        var propertyEntry = entityEntry.Property(property.Name);
                        var propertyChange = entityChange.PropertyChanges.FirstOrDefault(pc => pc.PropertyName == property.Name);

                        if (propertyChange == null)
                        {
                            if (propertyEntry.IsModified)
                            {
                                // Add foreign key
                                entityChange.PropertyChanges.Add(new TEntityPropertyChange
                                {
                                    NewValue = propertyEntry.CurrentValue.ToJsonString(),
                                    OriginalValue = propertyEntry.OriginalValue.ToJsonString(),
                                    PropertyName = property.Name,
                                    PropertyTypeFullName = property.ClrType.FullName
                                });
                            }

                            continue;
                        }

                        if (propertyChange.OriginalValue == propertyChange.NewValue)
                        {
                            var newValue = propertyEntry.CurrentValue.ToJsonString();
                            if (newValue == propertyChange.NewValue)
                            {
                                // No change
                                entityChange.PropertyChanges.Remove(propertyChange);
                            }
                            else
                            {
                                // Update foreign key
                                propertyChange.NewValue = newValue.TruncateWithPostfix(EntityPropertyChange<TKey>.MaxValueLength);
                            }
                        }
                    }
                }
            }
        }
    }
}
