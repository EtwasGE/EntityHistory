using EntityHistory.EntityFrameworkCore.Tests.Blogging.Domain;
using Microsoft.EntityFrameworkCore;

namespace EntityHistory.EntityFrameworkCore.Tests.Blogging
{
    public class BloggingDbContext : EntityHistoryDbContext
    {
        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<CommentFirst> FirstComments { get; set; }

        public DbSet<CommentSecond> SecondComments { get; set; }

        public BloggingDbContext(DbContextOptions<BloggingDbContext> options)
            : base(options)
        {
        }
    }
}