using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Notebooks.Commands.CreateNotebook;
using Ducode.Wolk.Application.Notebooks.Models;
using Ducode.Wolk.Common.Constants;
using Ducode.Wolk.TestUtilities.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using static Ducode.Wolk.TestUtilities.Assertions.NotebookAssertions;

namespace Ducode.Wolk.Api.Tests.Integration.Notebook
{
    [TestClass]
    public class Create : IntegrationTestBase
    {
        [TestMethod]
        public async Task Create_TokenIncorrect_ShouldReturn401()
        {
            // Arrange
            var url = "/api/notebook";

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var token = await GetJwt();
            request.AddJwtBearer(token + "a");

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task Create_ValidationError_ShouldReturn400()
        {
            // Arrange
            var url = "/api/notebook";

            var command = new CreateNotebookCommand {Name = new string('a', 201)};
            var request = new HttpRequestMessage(HttpMethod.Post, url)
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
        public async Task Create_HappyFlow()
        {
            // Arrange
            var url = "/api/notebook";

            var command = new CreateNotebookCommand {Name = Guid.NewGuid().ToString()};
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, MimeTypes.Json)
            };
            var token = await GetJwt();
            request.AddJwtBearer(token);

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            var returnedNotebook = JsonConvert.DeserializeObject<NotebookDto>(content);

            var notebook = await WolkDbContext.Notebooks.SingleAsync();
            ShouldBeEqual(notebook, returnedNotebook);
        }
    }
}
