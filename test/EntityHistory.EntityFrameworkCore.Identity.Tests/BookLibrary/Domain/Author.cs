using System.ComponentModel.DataAnnotations.Schema;
using EntityHistory.Configuration.Attributes;

namespace EntityHistory.EntityFrameworkCore.Identity.Tests.BookLibrary.Domain
{
    public class Author
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // ignore

        public string Name { get; set; } // default include

        [HistoryIgnore]
        public string Custom { get; set; }  // ignore
    }
}
