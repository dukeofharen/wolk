using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Notes.Commands.CreateNote;
using Ducode.Wolk.Application.Notes.Models;
using Ducode.Wolk.Common.Constants;
using Ducode.Wolk.TestUtilities.FakeData;
using Ducode.Wolk.TestUtilities.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using static Ducode.Wolk.TestUtilities.Assertions.NoteAssertions;

namespace Ducode.Wolk.Api.Tests.Integration.Note
{
    [TestClass]
    public class Create : IntegrationTestBase
    {
        [TestMethod]
        public async Task Create_TokenIncorrect_ShouldReturn401()
        {
            // Arrange
            var url = "/api/note";

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
            var url = "/api/note";
            var notebook = await WolkDbContext.CreateAndSaveNotebook();

            var command = new CreateNoteCommand
            {
                Title = new string('a', 201), Content = Guid.NewGuid().ToString(), NotebookId = notebook.Id
            };
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
        public async Task Create_NotebookNotFound_ShouldReturn404()
        {
            // Arrange
            var url = "/api/note";
            var notebook = await WolkDbContext.CreateAndSaveNotebook();

            var command = new CreateNoteCommand
            {
                Title = Guid.NewGuid().ToString(), Content = Guid.NewGuid().ToString(), NotebookId = notebook.Id + 1
            };
            var request = new HttpRequestMessage(HttpMethod.Post, url)
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
        public async Task Create_HappyFlow()
        {
            // Arrange
            var url = "/api/note";
            var notebook = await WolkDbContext.CreateAndSaveNotebook();

            var command = new CreateNoteCommand
            {
                Title = Guid.NewGuid().ToString(), Content = Guid.NewGuid().ToString(), NotebookId = notebook.Id
            };
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
            var returnedNote = JsonConvert.DeserializeObject<NoteDto>(content);

            var note = await WolkDbContext.Notes.SingleAsync();
            ShouldBeEqual(note, returnedNote);
        }
    }
}
