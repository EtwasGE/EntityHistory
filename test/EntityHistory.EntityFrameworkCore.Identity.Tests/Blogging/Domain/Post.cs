using System;
using System.ComponentModel.DataAnnotations;

namespace EntityHistory.EntityFrameworkCore.Identity.Tests.Blogging.Domain
{
    public class Post 
    {
        public Post()
        {
            Id = Guid.NewGuid();
        }

        public Post(Blog blog, string title, string body)
            : this()
        {
            Blog = blog;
            Title = title;
            Body = body;
        }

        public Guid Id { get; set; }
        
        public string Title { get; set; }

        public string Body { get; set; }

        [Required]
        public Blog Blog { get; set; }
    }
}
