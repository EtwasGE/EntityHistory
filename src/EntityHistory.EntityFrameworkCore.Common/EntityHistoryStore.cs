using System.Threading.Tasks;
using EntityHistory.Abstractions;

namespace EntityHistory.EntityFrameworkCore.Common
{
    public class EntityHistoryStore : IEntityHistoryStore
    {
        private readonly IEntityHistoryDbContext _context;

        public EntityHistoryStore(IEntityHistoryDbContext context)
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
