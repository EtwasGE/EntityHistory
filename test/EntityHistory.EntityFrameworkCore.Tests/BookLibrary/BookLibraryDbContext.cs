using EntityHistory.EntityFrameworkCore.Tests.BookLibrary.Domain;
using Microsoft.EntityFrameworkCore;

namespace EntityHistory.EntityFrameworkCore.Tests.BookLibrary
{
    public class BookLibraryDbContext : EntityHistoryDbContextBase<CustomEntityChangeSet, long, User>
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public BookLibraryDbContext(DbContextOptions<BookLibraryDbContext> options)
            : base(options)
        {
        }
    } 
}
