using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Ducode.Wolk.Application.NoteHistoryItems.Models;
using Ducode.Wolk.TestUtilities.FakeData;
using Ducode.Wolk.TestUtilities.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Ducode.Wolk.Api.Tests.Integration.Note
{
    [TestClass]
    public class GetNoteHistory : IntegrationTestBase
    {
        [TestMethod]
        public async Task GetNoteHistory_NoteNotFound_ShouldReturn404()
        {
            // Arrange
            var note = await WolkDbContext.CreateAndSaveNote();
            var url = $"/api/note/{note.Id + 1}/history";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var token = await GetJwt();
            request.AddJwtBearer(token);

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task GetNoteHistory_HappyFlow()
        {
            // Arrange
            var note1 = await WolkDbContext.CreateAndSaveNote();
            var note2 = await WolkDbContext.CreateAndSaveNote();
            var hist1 = await WolkDbContext.CreateAndSaveNoteHistory(note1);
            var hist2 = await WolkDbContext.CreateAndSaveNoteHistory(note1);
            var hist3 = await WolkDbContext.CreateAndSaveNoteHistory(note2);
            var url = $"/api/note/{note1.Id}/history";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var token = await GetJwt();
            request.AddJwtBearer(token);

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = JsonConvert.DeserializeObject<NoteHistoryDto[]>(await response.Content.ReadAsStringAsync());
            Assert.AreEqual(2, result.Length);
            Assert.IsTrue(result.All(h => h.Id == hist1.Id || h.Id == hist2.Id));
            Assert.IsTrue(result.All(h => h.NoteId == hist1.NoteId));
        }
    }
}
