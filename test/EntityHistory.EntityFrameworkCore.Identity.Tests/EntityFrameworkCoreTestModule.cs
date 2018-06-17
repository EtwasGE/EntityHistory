using EntityHistory.Core.Entities;
using EntityHistory.EntityFrameworkCore.TestBase;
using Microsoft.AspNetCore.Identity;

namespace EntityHistory.EntityFrameworkCore.Identity.Tests
{   
    public class EntityFrameworkCoreTestModule<TUser> : EntityFrameworkCoreTestModuleBase<EntityChangeSet<long, TUser>, long>
        where TUser : IdentityUser<long>
    {
    }
}
