using System.ComponentModel.DataAnnotations.Schema;

namespace EntityHistory.EntityFrameworkCore.Identity.Tests.Blogging.Domain
{
    public class CommentFirst : CommentFirstBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Body { get; set; }
    }

    public class CommentSecond : CommentSecondBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public float RatingScale { get; set; }
    }
    
    public abstract class CommentFirstBase
    { 
    }
    
    public abstract class CommentSecondBase
    {
    }
}
