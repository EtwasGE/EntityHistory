using EntityHistory.Configuration;

namespace EntityHistory.EntityFrameworkCore.Tests.BookLibrary
{
    public class BookLibraryEntityHistoryConfiguration : EntityHistoryConfiguration
    {
        public BookLibraryEntityHistoryConfiguration()
        {
            IsEnabledForAnonymousUsers = true;
        }
    }
}
