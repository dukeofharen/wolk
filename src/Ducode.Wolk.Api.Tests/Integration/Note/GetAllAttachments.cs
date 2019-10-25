using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Attachments.Models;
using Ducode.Wolk.Application.Notes.Models;
using Ducode.Wolk.TestUtilities.FakeData;
using Ducode.Wolk.TestUtilities.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using static Ducode.Wolk.TestUtilities.Assertions.AttachmentAssertions;

namespace Ducode.Wolk.Api.Tests.Integration.Note
{
    [TestClass]
    public class GetAllAttachments : IntegrationTestBase
    {
        [TestMethod]
        public async Task GetAllAttachments_HappyFlow()
        {
            // Arrange
            var note1 = await WolkDbContext.CreateAndSaveNote();
            var note2 = await WolkDbContext.CreateAndSaveNote();

            var file1 = await WolkDbContext.CreateAndSaveAttachment(note1);
            var file2 = await WolkDbContext.CreateAndSaveAttachment(note2);
            var file3 = await WolkDbContext.CreateAndSaveAttachment(note1);

            var url = $"/api/note/{note1.Id}/attachments";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var token = await GetJwt();
            request.AddJwtBearer(token);

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var attachments = JsonConvert.DeserializeObject<AttachmentDto[]>(content);

            Assert.AreEqual(2, attachments.Length);
            ShouldBeEqual(file1, attachments[0]);
            ShouldBeEqual(file3, attachments[1]);
        }
    }
}
