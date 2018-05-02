using System.ComponentModel.DataAnnotations.Schema;
using EntityHistory.Configuration.Attributes;

namespace EntityHistory.EntityFrameworkCore.Tests.BookLibrary.Domain
{
    [HistoryInclude]
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; } // ignore

        [HistoryIgnore]
        public string Name { get; set; } // ignore

        [HistoryOverride]
        public string Password { get; set; } // override
    }
}
