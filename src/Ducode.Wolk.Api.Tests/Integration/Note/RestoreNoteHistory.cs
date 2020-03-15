using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Ducode.Wolk.TestUtilities.FakeData;
using Ducode.Wolk.TestUtilities.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ducode.Wolk.Api.Tests.Integration.Note
{
    [TestClass]
    public class RestoreNoteHistory : IntegrationTestBase
    {
        [TestMethod]
        public async Task RestoreNoteHistory_HappyFlow()
        {
            // Arrange
            var note1 = await WolkDbContext.CreateAndSaveNote();
            var originalTitle = note1.Title;
            var originalContent = note1.Content;

            var note2 = await WolkDbContext.CreateAndSaveNote();
            var hist1 = await WolkDbContext.CreateAndSaveNoteHistory(note1);
            var hist2 = await WolkDbContext.CreateAndSaveNoteHistory(note1);
            var hist3 = await WolkDbContext.CreateAndSaveNoteHistory(note2);

            var url = $"/api/note/{note1.Id}/history/{hist2.Id}";

            var request = new HttpRequestMessage(HttpMethod.Put, url);
            var token = await GetJwt();
            request.AddJwtBearer(token);

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

            Assert.AreEqual(hist2.Title, note1.Title);
            Assert.AreEqual(hist2.Content, note1.Content);
            Assert.AreEqual(hist2.NoteType, note1.NoteType);

            var newHistory = await WolkDbContext.NoteHistory.LastAsync();
            Assert.AreEqual(hist3.Id + 1, newHistory.Id);
            Assert.AreEqual(originalTitle, newHistory.Title);
            Assert.AreEqual(originalContent, newHistory.Content);
        }
    }
}
