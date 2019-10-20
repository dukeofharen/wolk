using Ducode.Wolk.Infrastructure.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ducode.Wolk.Infrastructure.Tests
{
    [TestClass]
    public class MimeServiceTests
    {
        private readonly MimeService _service = new MimeService();

        [DataTestMethod]
        [DataRow("jpg", "image/jpeg")]
        [DataRow(".pdf", "application/pdf")]
        [DataRow("logo.png", "image/png")]
        [DataRow("plunky", "application/octet-stream")]
        public void GetMimeType_HappyFlow(string input, string expectedResult)
        {
            // Act
            var result = _service.GetMimeType(input);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
