using EntityHistory.Configuration;

namespace EntityHistory.EntityFrameworkCore.Tests.BookLibrary
{
    public class BookLibraryHistoryConfiguration : HistoryConfiguration
    {
        public BookLibraryHistoryConfiguration()
        {
            IsEnabledForAnonymousUsers = true;
        }
    }
}
