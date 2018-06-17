using EntityHistory.Configuration.Attributes;
using Microsoft.AspNetCore.Identity;

namespace EntityHistory.EntityFrameworkCore.Identity.Tests.BookLibrary.Domain
{
    [HistoryInclude]
    public class CustomUser : IdentityUser<long>
    {
    }
}
