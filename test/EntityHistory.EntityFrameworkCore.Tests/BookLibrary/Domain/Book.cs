using System;
using System.ComponentModel.DataAnnotations.Schema;
using EntityHistory.Configuration.Attributes;

namespace EntityHistory.EntityFrameworkCore.Tests.BookLibrary.Domain
{
    [HistoryInclude]
    public class Book
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }  // ignore

        public string Title { get; set; } // include

        public string Description { get; set; }  // include
    }
}
