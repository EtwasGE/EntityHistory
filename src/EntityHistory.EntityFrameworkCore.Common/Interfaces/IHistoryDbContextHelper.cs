using EntityHistory.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace EntityHistory.EntityFrameworkCore.Common.Interfaces
{
    public interface IHistoryDbContextHelper : IHistoryDbContextHelper<DbContext>
    {
    }
}
