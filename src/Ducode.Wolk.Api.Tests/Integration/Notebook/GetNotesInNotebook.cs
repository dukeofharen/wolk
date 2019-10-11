using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Notes.Models;
using Ducode.Wolk.TestUtilities.FakeData;
using Ducode.Wolk.TestUtilities.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using static Ducode.Wolk.TestUtilities.Assertions.NoteAssertsions;

namespace Ducode.Wolk.Api.Tests.Integration.Notebook
{
    [TestClass]
    public class GetNotesInNotebook : IntegrationTestBase
    {
        [TestMethod]
        public async Task GetNotesInNotebook_TokenIncorrect_ShouldReturn401()
        {
            // Arrange
            var url = "/api/notebook/1/notes";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var token = await GetJwt();
            request.AddJwtBearer(token + "a");

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task GetNotesInNotebook_HappyFlow()
        {
            // Arrange
            var notebook = await WolkDbContext.CreateAndSaveNotebook();
            var note1 = await WolkDbContext.CreateAndSaveNote(notebook);
            await WolkDbContext.CreateAndSaveNote();
            var note3 = await WolkDbContext.CreateAndSaveNote(notebook);

            var url = $"/api/notebook/{notebook.Id}/notes";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var token = await GetJwt();
            request.AddJwtBearer(token);

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var notes = JsonConvert.DeserializeObject<NoteDto[]>(content);

            Assert.AreEqual(2, notes.Length);
            ShouldBeEqual(note1, notes[0]);
            ShouldBeEqual(note3, notes[1]);
        }
    }
}
