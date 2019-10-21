using api.Models;
using Xunit;

namespace api.test.Unit
{
    public class Tests
    {
        [Fact]
        public void TestNewContributorProperties()
        {
            var contributor = new Contributor();

            Assert.True(string.IsNullOrEmpty(contributor.UserName));
            Assert.True(string.IsNullOrEmpty(contributor.LastName));
            Assert.True(string.IsNullOrEmpty(contributor.FirstName));
            Assert.True(string.IsNullOrEmpty(contributor.Email));
        }
    }
}
