using System;
using System.Collections.Generic;
using System.Linq;
using EntityHistory.Abstractions.Configuration;
using EntityHistory.Configuration;
using EntityHistory.Core;
using EntityHistory.Core.Entities;
using EntityHistory.Core.Extensions;
using EntityHistory.Core.History;
using EntityHistory.EntityFrameworkCore.Common.Extensions;
using EntityHistory.EntityFrameworkCore.Common.Interfaces;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EntityHistory.EntityFrameworkCore.Common
{
    public class EntityHistoryHelper : EntityHistoryHelper<long>
    {
        public EntityHistoryHelper(IEntityHistoryConfiguration configuration)
            : base(configuration)
        {
        }
    }
    
    public class EntityHistoryHelper<TUserKey> : EntityHistoryHelper<EntityChangeSet<TUserKey>, TUserKey>
        where TUserKey : struct, IEquatable<TUserKey>
    {
        public EntityHistoryHelper(IEntityHistoryConfiguration configuration)
            : base(configuration)
        {
        }
    }

    public class EntityHistoryHelper<TEntityChangeSet, TUserKey>
        : EntityHistoryHelperBase<EntityEntry, TEntityChangeSet, TUserKey>, IEntityHistoryHelper<TEntityChangeSet>
        where TEntityChangeSet : EntityChangeSet<TUserKey>, new()
        where TUserKey : struct, IEquatable<TUserKey>
    {
        public EntityHistoryHelper(IEntityHistoryConfiguration configuration)
            : base(configuration)
        {
        }

        protected override bool ShouldSaveEntityHistory(EntityEntry entityEntry)
        {
            if (entityEntry.State == EntityState.Detached ||
                entityEntry.State == EntityState.Unchanged)
            {
                return false;
            }

            var entityType = entityEntry.Metadata.ClrType;
            // check all base types
            if (IsIgnoredType(entityType))
            {
                return false;
            }

            if (Configuration.IgnoredTypes.Any(t => t.IsInstanceOfType(entityEntry.Entity)))
            {
                return false;
            }

            if (!entityType.IsPublic)
            {
                return false;
            }

            return GlobalConfigHelper.IsIncluded(entityType);
        }

        protected virtual bool IsIgnoredType(Type entityType)
        {
            if (entityType == null)
            {
                return false;
            }

            if (entityType.IsGenericType)
            {
                var entityGenericTypeDefinition = entityType.GetGenericTypeDefinition();
                if (Configuration.IgnoredTypes.Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == entityGenericTypeDefinition))
                {
                    return true;
                }
            }

            return IsIgnoredType(entityType.BaseType);
        }

        protected virtual bool ShouldSavePropertyHistory(PropertyEntry propertyEntry, bool defaultValue)
        {
            if (propertyEntry.Metadata.IsPrimaryKey())
            {
                return false;
            }

            var entityType = propertyEntry.EntityEntry.Entity.GetType();
            var propertyName = propertyEntry.Metadata.Name;

            if (!GlobalConfigHelper.IsIncluded(entityType, propertyName))
            {
                return false;
            };

            if (propertyEntry.IsModified)
            {
                return true;
            }

            return defaultValue;
        }

        [CanBeNull]
        protected override EntityChange GetEntityChange(EntityEntry entityEntry)
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

            var entityChange = new EntityChange
            {
                ChangeType = changeType,
                EntityEntry = entityEntry,
                EntityId = entityId,
                EntityTypeFullName = entityType.FullName,
                PropertyChanges = GetPropertyChanges(entityEntry).ToArray()
            };

            return entityChange;
        }

        protected virtual IEnumerable<EntityPropertyChange> GetPropertyChanges(EntityEntry entityEntry)
        {
            var propertyChanges = new List<EntityPropertyChange>();
            var properties = entityEntry.Metadata.GetProperties();
            var isCreated = entityEntry.IsCreated();
            var isDeleted = entityEntry.IsDeleted();

            foreach (var property in properties)
            {
                var propertyEntry = entityEntry.Property(property.Name);
                if (ShouldSavePropertyHistory(propertyEntry, isCreated || isDeleted))
                {
                    propertyChanges.Add(new EntityPropertyChange
                    {
                        NewValue = isDeleted ? null : propertyEntry.GetCurrentValue(),
                        OriginalValue = isCreated ? null : propertyEntry.GetOriginalValue(),
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
                                entityChange.PropertyChanges.Add(new EntityPropertyChange
                                {
                                    NewValue = propertyEntry.CurrentValue.ToString(),
                                    OriginalValue = propertyEntry.OriginalValue.ToString(),
                                    PropertyName = property.Name,
                                    PropertyTypeFullName = property.ClrType.FullName
                                });
                            }

                            continue;
                        }

                        if (propertyChange.OriginalValue == propertyChange.NewValue)
                        {
                            var newValue = propertyEntry.CurrentValue.ToString();
                            if (newValue == propertyChange.NewValue)
                            {
                                // No change
                                entityChange.PropertyChanges.Remove(propertyChange);
                            }
                            else
                            {
                                // Update foreign key
                                propertyChange.NewValue = newValue.TruncateWithPostfix(EntityPropertyChange.MaxValueLength);
                            }
                        }
                    }
                }
            }
        }
    }
}
