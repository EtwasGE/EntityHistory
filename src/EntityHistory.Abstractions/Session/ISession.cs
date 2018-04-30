using System;

namespace EntityHistory.Abstractions.Session
{
    /// <summary>
    /// Defines some session information that can be useful for applications.
    /// </summary>
    public interface ISession<TPrimaryKey>
        where TPrimaryKey: struct, IEquatable<TPrimaryKey>
    {
        /// <summary>
        /// Gets current UserId or null.
        /// It can be null if no user logged in.
        /// </summary>
        TPrimaryKey? UserId { get; }
    }
}
