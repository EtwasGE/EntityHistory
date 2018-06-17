using EntityHistory.EntityFrameworkCore.Identity.Tests.BookLibrary.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EntityHistory.EntityFrameworkCore.Identity.Tests.BookLibrary
{
    public class BookLibraryDbContext : HistoryDbContext<CustomUser, IdentityRole<long>, long>
    {
        public DbSet<Book> Books { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public BookLibraryDbContext(DbContextOptions<BookLibraryDbContext> options)
            : base(options)
        {
        }
    } 
}
