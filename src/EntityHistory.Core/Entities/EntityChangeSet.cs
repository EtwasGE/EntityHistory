using System;
using System.Collections.Generic;
using EntityHistory.Abstractions;

namespace EntityHistory.Core.Entities
{
    public class EntityChangeSet<TKey, TUser> : EntityChangeSet<TKey>
        where TKey : struct, IEquatable<TKey>
        where TUser : class
    {
        /// <summary>
        /// Change User.
        /// </summary>
        public virtual TUser User { get; set; }
    }

    public class EntityChangeSet<TKey> : IEntity<TKey> 
        where TKey : struct, IEquatable<TKey>
    {
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

        public virtual TKey Id { get; set; }

        /// <summary>
        /// Browser information if this entity is changed in a web request.
        /// </summary>
        public virtual string BrowserInfo { get; set; }

        /// <summary>
        /// IP address of the client.
        /// </summary>
        public virtual string ClientIpAddress { get; set; }
        
        /// <summary>
        /// Name (generally computer name) of the client.
        /// </summary>
        public virtual string ClientName { get; set; }

        /// <summary>
        /// Creation time of this entity.
        /// </summary>
        public virtual DateTime CreationTime { get; set; }

        /// <summary>
        /// Change UserId.
        /// </summary>
        public virtual TKey? UserId { get; set; }

        /// <summary>
        /// Entity changes grouped in this change set.
        /// </summary>
        public virtual ICollection<EntityChange<TKey>> EntityChanges { get; set; }
    }
}
