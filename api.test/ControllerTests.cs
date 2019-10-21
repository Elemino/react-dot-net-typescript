using api.Models;
using Xunit;

namespace api.test
{
    public class ControllerTests
    {
        [Fact]
        public void Test1()
        {
            var contact = new Contributor();
            Assert.True(string.IsNullOrEmpty(contact.Email));
        }
    }
}
