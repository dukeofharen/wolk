using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Notes.Models;
using Ducode.Wolk.TestUtilities.FakeData;
using Ducode.Wolk.TestUtilities.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using static Ducode.Wolk.TestUtilities.Assertions.NoteAssertions;

namespace Ducode.Wolk.Api.Tests.Integration.Note
{
    [TestClass]
    public class Get : IntegrationTestBase
    {
        [TestMethod]
        public async Task Get_NoteNotFound_ShouldReturn404()
        {
            // Arrange
            var note = await WolkDbContext.CreateAndSaveNote();
            var url = $"/api/note/{note.Id + 1}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var token = await GetJwt();
            request.AddJwtBearer(token);

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Get_HappyFlow()
        {
            // Arrange
            var note = await WolkDbContext.CreateAndSaveNote();
            var url = $"/api/note/{note.Id}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var token = await GetJwt();
            request.AddJwtBearer(token);

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var returnedNote = JsonConvert.DeserializeObject<NoteDto>(content);

            ShouldBeEqual(note, returnedNote);
        }
    }
}
