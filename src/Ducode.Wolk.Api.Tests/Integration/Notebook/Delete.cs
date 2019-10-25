using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Ducode.Wolk.TestUtilities.FakeData;
using Ducode.Wolk.TestUtilities.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ducode.Wolk.Api.Tests.Integration.Notebook
{
    [TestClass]
    public class Delete : IntegrationTestBase
    {
        [TestMethod]
        public async Task Delete_NotebookNotFound_ShouldReturn404()
        {
            // Arrange
            var notebook = await WolkDbContext.CreateAndSaveNotebook();
            var url = $"/api/notebook/{notebook.Id + 1}";

            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            var token = await GetJwt();
            request.AddJwtBearer(token);

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Delete_HappyFlow()
        {
            // Arrange
            var notebook = await WolkDbContext.CreateAndSaveNotebook();
            var url = $"/api/notebook/{notebook.Id}";

            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            var token = await GetJwt();
            request.AddJwtBearer(token);

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            Assert.IsFalse(await WolkDbContext.Notebooks.AnyAsync());
        }
    }
}
