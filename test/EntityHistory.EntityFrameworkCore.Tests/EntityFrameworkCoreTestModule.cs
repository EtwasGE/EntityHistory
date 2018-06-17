using System;
using EntityHistory.Core.Entities;
using EntityHistory.EntityFrameworkCore.TestBase;

namespace EntityHistory.EntityFrameworkCore.Tests
{
    public class EntityFrameworkCoreTestModule<TUserKey, TUser> : EntityFrameworkCoreTestModuleBase<EntityChangeSet<TUserKey, TUser>, TUserKey>
        where TUserKey : struct, IEquatable<TUserKey>
        where TUser : class
    {
    }

    public class EntityFrameworkCoreTestModule : EntityFrameworkCoreTestModule<long>
    {
    }

    public class EntityFrameworkCoreTestModule<TUserKey> : EntityFrameworkCoreTestModuleBase<EntityChangeSet<TUserKey>, TUserKey>
        where TUserKey : struct, IEquatable<TUserKey>
    {
    }
}
