using EntityHistory.Core.Entities;

namespace EntityHistory.EntityFrameworkCore.Tests.BookLibrary.Domain
{
    public class CustomEntityChangeSet : EntityChangeSet<long, User>
    {
        public string CustomProperty { get; set; }
    }
}
