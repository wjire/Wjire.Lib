using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wjire.Common;

namespace Wjire.CommonTests
{
    [TestClass()]
    public class HelloWorldHelperTest
    {
        [TestMethod]
        public void GetGreeting_ShouldReturnCorrectMessage()
        {
            // Act
            string greeting = HelloWorldHelper.GetGreeting();

            // Assert
            Assert.AreEqual("你好, World!", greeting);
        }
    }
}
