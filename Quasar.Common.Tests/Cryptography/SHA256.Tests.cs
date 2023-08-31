using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fahad.Common.Cryptography;
using Fahad.Common.Helpers;

namespace Fahad.Common.Tests.Cryptography
{
    [TestClass]
    public class Sha256Tests
    {
        [TestMethod, TestCategory("Cryptography")]
        public void ComputeHashTest()
        {
            var input = StringHelper.GetRandomString(100);
            var result = Sha256.ComputeHash(input);

            Assert.IsNotNull(result);
            Assert.AreNotEqual(result, input);
        }
    }
}
