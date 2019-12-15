using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ducode.Wolk.Api.Models.Users;
using Ducode.Wolk.Application.Users.Models;
using Ducode.Wolk.Common.Constants;
using Ducode.Wolk.TestUtilities.FakeData;
using Ducode.Wolk.TestUtilities.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Ducode.Wolk.Api.Tests.Integration.User
{
    [TestClass]
    public class Login : IntegrationTestBase
    {
        [TestMethod]
        public async Task Authenticate_CredentialsIncorrect_ShouldReturn401()
        {
            // Arrange
            var url = "/api/user/authenticate";
            var user = await WolkDbContext.CreateAndSaveUser();
            var model = new SignInModel {Email = user.Email, Password = "adfsdfgsdfgdf"};
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, MimeTypes.Json)
            };

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task Authenticate_CredentialsCorrect()
        {
            // Arrange
            var url = "/api/user/authenticate";
            var user = await WolkDbContext.CreateAndSaveUser();
            var model = new SignInModel {Email = user.Email, Password = "Pass123"};
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, MimeTypes.Json)
            };

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var viewModel = JsonConvert.DeserializeObject<SignedInViewModel>(content);
            Assert.AreEqual(user.Id, viewModel.Id);
            Assert.AreEqual(user.Email, viewModel.Email);
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var result = new JwtSecurityTokenHandler().ValidateToken(viewModel.Token,
                new TokenValidationParameters
                {
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IdentityConfiguration.JwtSecret)),
                    ValidateAudience = false,
                    ValidateIssuer = false
                }, out var validatedToken);

            Assert.AreEqual(user.Id.ToString(), result.Claims.Single(c => c.Type == "sub").Value);

            // Act: do a call to get all notebooks with correct user token
            url = "/api/notebook";

            request = new HttpRequestMessage(HttpMethod.Get, url);
            request.AddJwtBearer(viewModel.Token);
            using var notebookResponse = await HttpClient.SendAsync(request);

            // Assert
            Assert.IsTrue(notebookResponse.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task Authenticate_CredentialsCorrect_UserIsDeleted_ShouldReturn401()
        {
            // Arrange
            var url = "/api/user/authenticate";
            var user = await WolkDbContext.CreateAndSaveUser();
            var model = new SignInModel {Email = user.Email, Password = "Pass123"};
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, MimeTypes.Json)
            };

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var viewModel = JsonConvert.DeserializeObject<SignedInViewModel>(content);

            // Act: delete user
            WolkDbContext.Users.Remove(user);
            await WolkDbContext.SaveChangesAsync();

            // Act: do a call to get all notebooks with "deleted" user token
            url = "/api/notebook";

            request = new HttpRequestMessage(HttpMethod.Get, url);
            request.AddJwtBearer(viewModel.Token);
            using var notebookResponse = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, notebookResponse.StatusCode);
        }

        [TestMethod]
        public async Task Authenticate_CredentialsCorrect_RehashPassword()
        {
            // Arrange
            var pass = "Pass123";
            var hash = PasswordUtilities.CreateDeprecatedPasswordHash(pass);
            var url = "/api/user/authenticate";
            var user = await WolkDbContext.CreateAndSaveUser(u => u.PasswordHash = hash);
            var model = new SignInModel {Email = user.Email, Password = pass};
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, MimeTypes.Json)
            };

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // Check that the password hash has changed
            Assert.AreNotEqual(hash, user.PasswordHash);

            var passwordHasher = new PasswordHasher<Domain.Entities.User>();
            Assert.AreEqual(
                PasswordVerificationResult.Success,
                passwordHasher.VerifyHashedPassword(user, user.PasswordHash, pass));
        }
    }
}
