using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Ducode.Wolk.TestUtilities.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ducode.Wolk.Api.Tests.Integration.General
{
    [TestClass]
    public class TokenRenewal : IntegrationTestBase
    {
        [TestMethod]
        public async Task TokenAlmostExpired_ShouldCreateAndReturnNewToken()
        {
            // Arrange
            // Set the current date and time to the past for creating an almost expired token.
            var past = DateTime.Now.AddMinutes(-59);
            SetLocalDateTime(past);
            var jwt = await GetJwt();
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/notebook");
            request.AddJwtBearer(jwt);

            var now = DateTime.Now;
            SetLocalDateTime(now);

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var newToken = response.Headers.Single(h => h.Key == "Token").Value.Single();
            Assert.IsFalse(string.IsNullOrWhiteSpace(newToken));

            // Assert that the new token is valid
            var newTokenRequest = new HttpRequestMessage(HttpMethod.Get, "/api/notebook");
            newTokenRequest.AddJwtBearer(newToken);
            using var newTokenResponse = await HttpClient.SendAsync(newTokenRequest);
            newTokenResponse.EnsureSuccessStatusCode();
        }

        [TestMethod]
        public async Task TokenNotAlmostExpired_ShouldNotCreateAndReturnNewToken()
        {
            // Arrange
            // Set the current date and time to the past for creating an almost expired token.
            var past = DateTime.Now.AddMinutes(-30);
            SetLocalDateTime(past);
            var jwt = await GetJwt();
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/notebook");
            request.AddJwtBearer(jwt);

            var now = DateTime.Now;
            SetLocalDateTime(now);

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.IsFalse(response.Headers.Any(h => h.Key == "Token"));
        }
    }
}
