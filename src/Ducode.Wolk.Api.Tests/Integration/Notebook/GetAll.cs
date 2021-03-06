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
    public class GetAll : IntegrationTestBase
    {
        [TestMethod]
        public async Task GetAll_HappyFlow()
        {
            // Arrange
            var notebook1 = await WolkDbContext.CreateAndSaveNotebook();
            var notebook2 = await WolkDbContext.CreateAndSaveNotebook();
            var url = "/api/notebook";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var token = await GetJwt();
            request.AddJwtBearer(token);

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var notebooks = JsonConvert.DeserializeObject<NotebookDto[]>(content);

            ShouldBeEqual(notebook1, notebooks[0]);
            ShouldBeEqual(notebook2, notebooks[1]);
        }
    }
}
