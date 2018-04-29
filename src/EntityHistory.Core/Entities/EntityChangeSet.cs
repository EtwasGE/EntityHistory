using System;
using System.Collections.Generic;
using EntityHistory.Core.Interfaces;

//namespace EntityHistory.Core.Entities.Principal
//{
//    public class EntityChangeSet<TUser> : EntityChangeSet<long, TUser>, IEntity
//        where TUser : class
//    {
//    }
//}

namespace EntityHistory.Core.Entities
{
    //public class EntityChangeSet : EntityChangeSet<long>, IEntity
    //{
    //}

    public class EntityChangeSet<TPrimaryKey, TUser> : EntityChangeSet<TPrimaryKey>
        where TPrimaryKey : struct, IEquatable<TPrimaryKey>
        where TUser : class
    {
        /// <summary>
        /// Change User.
        /// </summary>
        public virtual TUser User { get; set; }
    }

    public class EntityChangeSet<TPrimaryKey> : IEntity<TPrimaryKey> 
        where TPrimaryKey : struct, IEquatable<TPrimaryKey>
    {
        public virtual TPrimaryKey Id { get; set; }

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
        /// Change time.
        /// </summary>
        public virtual DateTime ChangeTime { get; set; }

        /// <summary>
        /// Change UserId.
        /// </summary>
        public virtual TPrimaryKey? UserId { get; set; }

        /// <summary>
        /// Entity changes grouped in this change set.
        /// </summary>
        public virtual ICollection<EntityChange<TPrimaryKey>> EntityChanges { get; set; }
    }
}
