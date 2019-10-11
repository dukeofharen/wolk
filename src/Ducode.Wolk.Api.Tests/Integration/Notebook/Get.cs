using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Notebooks.Models;
using Ducode.Wolk.TestUtilities.FakeData;
using Ducode.Wolk.TestUtilities.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using static Ducode.Wolk.TestUtilities.Assertions.NotebookAssertions;

namespace Ducode.Wolk.Api.Tests.Integration.Notebook
{
    [TestClass]
    public class Get : IntegrationTestBase
    {
        [TestMethod]
        public async Task Get_TokenIncorrect_ShouldReturn401()
        {
            // Arrange
            var url = "/api/notebook/1";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var token = await GetJwt();
            request.AddJwtBearer(token + "a");

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task Get_NotebookNotFound_ShouldReturn404()
        {
            // Arrange
            var notebook = await WolkDbContext.CreateAndSaveNotebook();
            var url = $"/api/notebook/{notebook.Id + 1}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var token = await GetJwt();
            request.AddJwtBearer(token);

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Get_HappyFlow()
        {
            // Arrange
            var notebook = await WolkDbContext.CreateAndSaveNotebook();
            var url = $"/api/notebook/{notebook.Id}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var token = await GetJwt();
            request.AddJwtBearer(token);

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var returnedNotebook = JsonConvert.DeserializeObject<NotebookDto>(content);

            ShouldBeEqual(notebook, returnedNotebook);
        }
    }
}
