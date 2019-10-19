using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ducode.Wolk.Api.Models.Notes;
using Ducode.Wolk.Common.Constants;
using Ducode.Wolk.Domain.Entities.Enums;
using Ducode.Wolk.TestUtilities.FakeData;
using Ducode.Wolk.TestUtilities.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using static Ducode.Wolk.TestUtilities.Assertions.NoteAssertions;

namespace Ducode.Wolk.Api.Tests.Integration.Note
{
    [TestClass]
    public class Update : IntegrationTestBase
    {
        [TestMethod]
        public async Task Update_TokenIncorrect_ShouldReturn401()
        {
            // Arrange
            var url = "/api/note/1";

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
            var url = "/api/note/1";
            var notebook = await WolkDbContext.CreateAndSaveNotebook();

            var model = new MutateNoteModel
            {
                Title = new string('a', 201), Content = Guid.NewGuid().ToString(), NotebookId = notebook.Id
            };
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
        public async Task Update_NotebookNotFound_ShouldReturn400()
        {
            // Arrange
            var url = "/api/note/1";
            var notebook = await WolkDbContext.CreateAndSaveNotebook();

            var model = new MutateNoteModel
            {
                Title = Guid.NewGuid().ToString(), Content = Guid.NewGuid().ToString(), NotebookId = notebook.Id + 1
            };
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
        public async Task Update_NoteNotFound_ShouldReturn404()
        {
            // Arrange
            var note = await WolkDbContext.CreateAndSaveNote();
            var url = $"/api/note/{note.Id + 1}";
            var notebook = await WolkDbContext.CreateAndSaveNotebook();

            var command = new MutateNoteModel
            {
                Title = Guid.NewGuid().ToString(),
                Content = Guid.NewGuid().ToString(),
                NotebookId = notebook.Id,
                NoteType = NoteType.Markdown
            };
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
            var notebook1 = await WolkDbContext.CreateAndSaveNotebook();
            var notebook2 = await WolkDbContext.CreateAndSaveNotebook();

            var note = await WolkDbContext.CreateAndSaveNote(notebook1);
            var url = $"/api/note/{note.Id}";

            var model = new MutateNoteModel
            {
                Title = Guid.NewGuid().ToString(),
                Content = Guid.NewGuid().ToString(),
                NotebookId = notebook2.Id,
                NoteType = NoteType.Markdown
            };
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
            ShouldBeEqual(note, model);
        }
    }
}
