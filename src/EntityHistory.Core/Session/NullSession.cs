using System;
using EntityHistory.Abstractions.Session;

namespace EntityHistory.Core.Session
{
    public sealed class NullSession<TKey> : ISession<TKey> 
        where TKey : struct, IEquatable<TKey>
    {
        public static NullSession<TKey> Instance { get; } = new NullSession<TKey>();

        public TKey? UserId => null;
    }
}
