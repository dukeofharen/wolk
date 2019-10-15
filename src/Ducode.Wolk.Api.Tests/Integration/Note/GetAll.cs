using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Notebooks.Models;
using Ducode.Wolk.Application.Notes.Models;
using Ducode.Wolk.TestUtilities.FakeData;
using Ducode.Wolk.TestUtilities.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using static Ducode.Wolk.TestUtilities.Assertions.NoteAssertions;

namespace Ducode.Wolk.Api.Tests.Integration.Note
{
    [TestClass]
    public class GetAll : IntegrationTestBase
    {
        [TestMethod]
        public async Task GetAll_TokenIncorrect_ShouldReturn401()
        {
            // Arrange
            var url = "/api/note";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var token = await GetJwt();
            request.AddJwtBearer(token + "a");

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task GetAll_HappyFlow()
        {
            // Arrange
            var note1 = await WolkDbContext.CreateAndSaveNote();
            var note2 = await WolkDbContext.CreateAndSaveNote();
            var url = "/api/note";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var token = await GetJwt();
            request.AddJwtBearer(token);

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var notes = JsonConvert.DeserializeObject<NoteOverviewDto[]>(content);

            ShouldBeEqual(note1, notes[0]);
            ShouldBeEqual(note2, notes[1]);
        }

        [TestMethod]
        public async Task GetAll_HappyFlow_NotebookIdSet()
        {
            // Arrange
            var notebook1 = await WolkDbContext.CreateAndSaveNotebook();
            var notebook2 = await WolkDbContext.CreateAndSaveNotebook();

            var note1 = await WolkDbContext.CreateAndSaveNote(notebook1);
            var note2 = await WolkDbContext.CreateAndSaveNote(notebook2);
            var note3 = await WolkDbContext.CreateAndSaveNote(notebook1);
            var url = $"/api/note?notebookId={notebook1.Id}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var token = await GetJwt();
            request.AddJwtBearer(token);

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var notes = JsonConvert.DeserializeObject<NoteOverviewDto[]>(content);

            Assert.AreEqual(2, notes.Length);

            ShouldBeEqual(note1, notes[0]);
            ShouldBeEqual(note3, notes[1]);
        }
    }
}
