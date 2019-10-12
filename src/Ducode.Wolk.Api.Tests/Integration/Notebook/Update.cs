using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Notebooks.Commands.UpdateNotebook;
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
        public async Task Update_TokenIncorrect_ShouldReturn401()
        {
            // Arrange
            var url = "/api/notebook/1";

            var request = new HttpRequestMessage(HttpMethod.Put, url);
            var token = await GetJwt();
            request.AddJwtBearer(token + "a");

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_ValidationError_ShouldReturn400()
        {
            // Arrange
            var url = "/api/notebook/1";

            var command = new UpdateNotebookCommand {Name = new string('a', 201)};
            var request = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, MimeTypes.Json)
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

            var command = new UpdateNotebookCommand {Name = Guid.NewGuid().ToString()};
            var request = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, MimeTypes.Json)
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

            var command = new UpdateNotebookCommand {Name = Guid.NewGuid().ToString()};
            var request = new HttpRequestMessage(HttpMethod.Put, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, MimeTypes.Json)
            };
            var token = await GetJwt();
            request.AddJwtBearer(token);

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            Assert.AreEqual(command.Name, notebook.Name);
        }
    }
}
