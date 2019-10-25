using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ducode.Wolk.Api.Models.Notebooks;
using Ducode.Wolk.Common.Constants;
using Ducode.Wolk.TestUtilities.FakeData;
using Ducode.Wolk.TestUtilities.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Ducode.Wolk.Api.Tests.Integration.Notebook
{
    [TestClass]
    public class Update : IntegrationTestBase
    {
        [TestMethod]
        public async Task Update_ValidationError_ShouldReturn400()
        {
            // Arrange
            var url = "/api/notebook/1";

            var model = new MutateNotebookModel {Name = new string('a', 201)};
            var request = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, MimeTypes.Json)
            };
            var token = await GetJwt();
            request.AddJwtBearer(token);

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_NotebookNotFound_ShouldReturn404()
        {
            // Arrange
            var notebook = await WolkDbContext.CreateAndSaveNotebook();
            var url = $"/api/notebook/{notebook.Id + 1}";

            var model = new MutateNotebookModel {Name = Guid.NewGuid().ToString()};
            var request = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, MimeTypes.Json)
            };
            var token = await GetJwt();
            request.AddJwtBearer(token);

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }


        [TestMethod]
        public async Task Update_HappyFlow()
        {
            // Arrange
            var notebook = await WolkDbContext.CreateAndSaveNotebook();
            var url = $"/api/notebook/{notebook.Id}";

            var model = new MutateNotebookModel {Name = Guid.NewGuid().ToString()};
            var request = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, MimeTypes.Json)
            };
            var token = await GetJwt();
            request.AddJwtBearer(token);

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            Assert.AreEqual(model.Name, notebook.Name);
        }
    }
}
