using System.IO;
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
    public class DeleteAttachment : IntegrationTestBase
    {
        [TestMethod]
        public async Task DeleteAttachment_TokenIncorrect_ShouldReturn401()
        {
            // Arrange
            var url = "/api/note/1/attachments/1";

            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            var token = await GetJwt();
            request.AddJwtBearer(token + "a");

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task DeleteAttachment_NoteNotFound_ShouldReturn404()
        {
            // Arrange
            var attachment = await WolkDbContext.CreateAndSaveAttachment();
            var url = $"/api/note/1/attachments/{attachment.Id + 1}";

            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            var token = await GetJwt();
            request.AddJwtBearer(token);

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Delete_HappyFlow()
        {
            // Arrange
            var attachment = await WolkDbContext.CreateAndSaveAttachment();
            var url = $"/api/note/1/attachments/{attachment.Id}";

            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            var token = await GetJwt();
            request.AddJwtBearer(token);

            var expectedPath = Path.Combine(UploadsRootPath, attachment.InternalFilename);
            MockFileService
                .Setup(m => m.FileExists(expectedPath))
                .Returns(true);

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            Assert.IsFalse(await WolkDbContext.Attachments.AnyAsync());
            MockFileService
                .Verify(m => m.DeleteFile(expectedPath));
        }
    }
}
