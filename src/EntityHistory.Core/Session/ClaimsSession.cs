using System;
using System.Linq;
using System.Security.Claims;
using EntityHistory.Abstractions.Session;

namespace EntityHistory.Core.Session
{
    /// <summary>
    /// Implements <see cref="ISession{TKey}"/> to get session properties from current claims.
    /// </summary>
    public class ClaimsSession<TKey> : ISession<TKey> 
        where TKey : struct, IEquatable<TKey>
    {
        public ClaimsSession(IPrincipalAccessor principalAccessor) 
        {
            PrincipalAccessor = principalAccessor;
        }

        protected IPrincipalAccessor PrincipalAccessor { get; }

        public virtual TKey? UserId
        {
            get
            {
                var userIdClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdClaim?.Value))
                {
                    return null;
                }

                //TODO: check
                return (TKey)Convert.ChangeType(userIdClaim, typeof(TKey));
            }
        }
    }
}
