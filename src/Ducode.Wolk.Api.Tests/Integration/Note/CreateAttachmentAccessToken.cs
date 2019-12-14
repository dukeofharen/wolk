using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ducode.Wolk.Api.Models.Attachments;
using Ducode.Wolk.Application.AccessTokens.Models;
using Ducode.Wolk.Common.Constants;
using Ducode.Wolk.TestUtilities.FakeData;
using Ducode.Wolk.TestUtilities.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Ducode.Wolk.Api.Tests.Integration.Note
{
    [TestClass]
    public class CreateAttachmentAccessToken : IntegrationTestBase
    {
        [TestMethod]
        public async Task CreateAttachmentAccessToken_HappyFlow()
        {
            // Arrange
            var attachment = await WolkDbContext.CreateAndSaveAttachment();
            var model = new MutateAttachmentAccessTokenModel
            {
                ExpirationDateTime = new DateTimeOffset(2019, 12, 31, 23, 0, 0, TimeSpan.FromHours(2))
            };

            var url = $"/api/note/{attachment.NoteId}/attachments/{attachment.Id}/accessTokens";

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
            var returnedToken = JsonConvert.DeserializeObject<AccessTokenResultDto>(content);

            Assert.AreEqual(model.ExpirationDateTime, returnedToken.ExpirationDateTime);
            Assert.IsTrue(Guid.TryParse(returnedToken.Token, out var _));

            var addedToken = await WolkDbContext.AccessTokens.SingleAsync();
            Assert.AreEqual(returnedToken.Token, addedToken.Token);
            Assert.AreEqual(returnedToken.ExpirationDateTime, addedToken.ExpirationDateTime);
        }
    }
}
