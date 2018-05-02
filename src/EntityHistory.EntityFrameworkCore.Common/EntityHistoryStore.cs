using System.Threading.Tasks;
using EntityHistory.Abstractions;
using EntityHistory.EntityFrameworkCore.Common.Interfaces;

namespace EntityHistory.EntityFrameworkCore.Common
{
    public class EntityHistoryStore : IEntityHistoryStore
    {
        private readonly IDbContext _context;

        public EntityHistoryStore(IDbContext context)
        {
            _context = context;
        }

        public async Task SaveAsync<TEntityChangeSet>(TEntityChangeSet entityChangeSet)
            where TEntityChangeSet : class
        {
            await _context.Set<TEntityChangeSet>().AddAsync(entityChangeSet);
            await _context.SaveChangesAsync();
        }
    }
}
