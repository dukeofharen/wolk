using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ducode.Wolk.Api.Tests.Integration.Notebook
{
    [TestClass]
    public class GetAll : IntegrationTestBase
    {
        [TestInitialize]
        public void Setup() => InitializeIntegrationTest();

        [TestCleanup]
        public void Cleanup() => CleanupIntegrationTest();

        [TestMethod]
        public async Task GetAll_TokenIncorrect_ShouldReturn401()
        {
            // Arrange
            var url = "/api/notebook";

            // Act
            using var response = await HttpClient.GetAsync(url);
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
