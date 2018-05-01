using EntityHistory.EntityFrameworkCore.Tests.Domain;
using Microsoft.EntityFrameworkCore;

namespace EntityHistory.EntityFrameworkCore.Tests.Ef
{
    public class BloggingDbContext : EntityHistoryDbContext
    {
        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Post> Posts { get; set; }

        public BloggingDbContext(DbContextOptions<BloggingDbContext> options)
            : base(options)
        {
        }
    }
}