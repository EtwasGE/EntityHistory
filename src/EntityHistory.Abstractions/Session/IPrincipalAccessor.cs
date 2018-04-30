using System.Security.Claims;

namespace EntityHistory.Abstractions.Session
{
    public interface IPrincipalAccessor
    {
        ClaimsPrincipal Principal { get; }
    }
}
