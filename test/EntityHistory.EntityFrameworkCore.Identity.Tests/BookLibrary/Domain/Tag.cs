using System.ComponentModel.DataAnnotations.Schema;
using EntityHistory.Configuration.Attributes;

namespace EntityHistory.EntityFrameworkCore.Identity.Tests.BookLibrary.Domain
{
    public class Tag : TagBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; } // include
    }

    public class TagBase
    {
        [HistoryOverride("New Custom")]
        public string Custom { get; set; } // override
    }
}
