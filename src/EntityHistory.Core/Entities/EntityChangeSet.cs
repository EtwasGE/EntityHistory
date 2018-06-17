using System;
using System.Collections.Generic;

namespace EntityHistory.Core.Entities
{
    public class EntityChangeSet<TUserKey, TUser> : EntityChangeSet<TUserKey>
        where TUserKey : struct, IEquatable<TUserKey>
        where TUser : class
    {
        /// <summary>
        /// Change User.
        /// </summary>
        public virtual TUser User { get; set; }
    }

    public class EntityChangeSet<TUserKey>
        where TUserKey : struct, IEquatable<TUserKey>
    {
        public EntityChangeSet()
        {
            EntityChanges = new HashSet<EntityChange>();
        }

        /// <summary>
        /// Maximum length of <see cref="EntityChangeSet{TKey}.BrowserInfo"/> property.
        /// </summary>
        public const int MaxBrowserInfoLength = 512;

        /// <summary>
        /// Maximum length of <see cref="EntityChangeSet{TKey}.ClientIpAddress"/> property.
        /// </summary>
        public const int MaxClientIpAddressLength = 64;

        /// <summary>
        /// Maximum length of <see cref="EntityChangeSet{TKey}.ClientName"/> property.
        /// </summary>
        public const int MaxClientNameLength = 128;

        /// <summary>
        /// Primary Key
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Browser information if this entity is changed in a web request.
        /// </summary>
        public string BrowserInfo { get; set; }

        /// <summary>
        /// IP address of the client.
        /// </summary>
        public string ClientIpAddress { get; set; }
        
        /// <summary>
        /// Name (generally computer name) of the client.
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Creation time of this entity.
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Change UserId.
        /// </summary>
        public TUserKey? UserId { get; set; }

        /// <summary>
        /// Entity changes grouped in this change set.
        /// </summary>
        public virtual ICollection<EntityChange> EntityChanges { get; set; }
    }
}
