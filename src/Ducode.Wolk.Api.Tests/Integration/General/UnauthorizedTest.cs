using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Ducode.Wolk.TestUtilities.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ducode.Wolk.Api.Tests.Integration.General
{
    [TestClass]
    public class UnauthorizedTest : IntegrationTestBase
    {
        [DataTestMethod]
        [DataRow("/api/note", "POST")]
        [DataRow("/api/note/1/attachments", "POST")]
        [DataRow("/api/note/1", "DELETE")]
        [DataRow("/api/note/1/attachments/1", "DELETE")]
        [DataRow("/api/note/1", "GET")]
        [DataRow("/api/note", "GET")]
        [DataRow("/api/note/1/attachments", "GET")]
        [DataRow("/api/note/1/attachments/1", "GET")]
        [DataRow("/api/note/1/attachments/1/accessTokens", "POST")]
        [DataRow("/api/note/1", "PUT")]
        [DataRow("/api/notebook", "POST")]
        [DataRow("/api/notebook/1", "DELETE")]
        [DataRow("/api/notebook/1", "GET")]
        [DataRow("/api/notebook", "GET")]
        [DataRow("/api/notebook/1", "PUT")]
        public async Task CheckUnauthorized(string url, string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), url);
            var token = await GetJwt();
            request.AddJwtBearer(token + "a");

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
