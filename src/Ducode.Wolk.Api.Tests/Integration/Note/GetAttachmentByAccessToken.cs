using System.IO;
using System.Linq;
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
    public class GetAttachmentByAccessToken : IntegrationTestBase
    {
        [TestMethod]
        public async Task GetAttachmentByAccessToken_AttachmentNotFound_ShouldReturn404()
        {
            // Arrange
            var attachment = await WolkDbContext.CreateAndSaveAttachment();
            var accessToken = await WolkDbContext.CreateAndSaveAttachmentAccessToken(attachment);
            var token = accessToken.Token + "1";
            var url = $"/api/note/1/attachments/{attachment.Id + 1}/accessTokens/{token}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var jwt = await GetJwt();
            request.AddJwtBearer(jwt);

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task GetAttachmentByAccessToken_HappyFlow()
        {
            // Arrange
            var attachment = await WolkDbContext.CreateAndSaveAttachment();
            var accessToken = await WolkDbContext.CreateAndSaveAttachmentAccessToken(attachment);
            var token = accessToken.Token;
            var url = $"/api/note/1/attachments/{attachment.Id + 1}/accessTokens/{token}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var jwt = await GetJwt();
            request.AddJwtBearer(jwt);

            var path = Path.Combine(UploadsRootPath, attachment.InternalFilename);
            var uploadedFile = new byte[] {3, 4, 1, 6, 12};
            MockFileService
                .Setup(m => m.FileExists(path))
                .Returns(true);
            MockFileService
                .Setup(m => m.ReadAllBytes(path))
                .Returns(uploadedFile);

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsByteArrayAsync();
            Assert.IsTrue(uploadedFile.SequenceEqual(content));
        }
    }
}
