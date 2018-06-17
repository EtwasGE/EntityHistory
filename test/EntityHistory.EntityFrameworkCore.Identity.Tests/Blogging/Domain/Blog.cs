using System;
using System.Collections.Generic;
using EntityHistory.Core.Helpers;

namespace EntityHistory.EntityFrameworkCore.Identity.Tests.Blogging.Domain
{
    public class Blog 
    {
        public Blog()
        {
            Id = Guid.NewGuid();
        }

        public Blog(string name, string url)
            :this()
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNullOrWhiteSpace(url, nameof(url));

            Name = name;
            Url = url;
        }

        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string Url { get; set; }

        public int Raiting { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
