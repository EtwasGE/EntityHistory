using System;

namespace EntityHistory.Core.Interfaces
{
    public interface IEntityChangeSet
    {
        /// <summary>
        /// Browser information if this entity is changed in a web request.
        /// </summary>
        string BrowserInfo { get; set; }

        /// <summary>
        /// IP address of the client.
        /// </summary>
        string ClientIpAddress { get; set; }

        /// <summary>
        /// Name (generally computer name) of the client.
        /// </summary>
        string ClientName { get; set; }

        /// <summary>
        /// Creation time of this entity.
        /// </summary>
        DateTime CreationTime { get; set; }
        
        /// <summary>
        /// UserId.
        /// </summary>
        long? UserId { get; set; }
    }

    public interface IEntityChangeSet<TUser> : IEntityChangeSet
    {
        /// <summary>
        /// User.
        /// </summary>
        TUser User { get; set; }
    }
}
