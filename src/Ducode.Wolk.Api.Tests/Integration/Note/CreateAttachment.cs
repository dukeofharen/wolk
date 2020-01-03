using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ducode.Wolk.Api.Models.Attachments;
using Ducode.Wolk.Application.Attachments.Models;
using Ducode.Wolk.Common.Constants;
using Ducode.Wolk.TestUtilities.FakeData;
using Ducode.Wolk.TestUtilities.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using static Ducode.Wolk.TestUtilities.Assertions.AttachmentAssertions;

namespace Ducode.Wolk.Api.Tests.Integration.Note
{
    [TestClass]
    public class CreateAttachment : IntegrationTestBase
    {
        [TestMethod]
        public async Task CreateAttachment_ValidationError_ShouldReturn400()
        {
            // Arrange
            var note = await WolkDbContext.CreateAndSaveNote();
            var contents = new byte[] {1, 2, 3, 4};
            var model = new MutateAttachmentModel
            {
                Filename = new string('a', 301), Base64Contents = Convert.ToBase64String(contents)
            };

            var url = $"/api/note/{note.Id}/attachments";

            var request = new HttpRequestMessage(HttpMethod.Post, url)
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
        public async Task CreateAttachment_HappyFlow()
        {
            // Arrange
            var note = await WolkDbContext.CreateAndSaveNote();
            var contents = new byte[] {1, 2, 3, 4};
            var model = new MutateAttachmentModel
            {
                Filename = "file.txt", Base64Contents = Convert.ToBase64String(contents)
            };

            var url = $"/api/note/{note.Id}/attachments";

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, MimeTypes.Json)
            };
            var token = await GetJwt();
            request.AddJwtBearer(token);

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            var returnedAttachment = JsonConvert.DeserializeObject<AttachmentDto>(content);

            var attachment = await WolkDbContext.Attachments.SingleAsync();
            ShouldBeEqual(attachment, returnedAttachment);

            Assert.AreEqual(1, MockFileService.Files.Count);
        }
    }
}
