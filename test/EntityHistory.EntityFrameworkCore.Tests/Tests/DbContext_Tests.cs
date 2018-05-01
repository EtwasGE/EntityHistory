using Xunit;

namespace EntityHistory.EntityFrameworkCore.Tests.Tests
{
    public class DbContext_Tests : EntityFrameworkCoreTestBase
    {
        [Fact]
        public void CustomTestMethod()
        {
            Assert.NotNull(Session);
        }
    }
}
