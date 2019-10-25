using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Users.Commands.CreateUser;
using Ducode.Wolk.Common.Constants;
using Ducode.Wolk.TestUtilities.FakeData;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using static Ducode.Wolk.TestUtilities.Assertions.UserAssertions;

namespace Ducode.Wolk.Api.Tests.Integration.User
{
    [TestClass]
    public class Register : IntegrationTestBase
    {
        [TestMethod]
        public async Task Register_ValidationErrors_ShouldReturn400()
        {
            // Arrange
            var url = "/api/user";
            var command = new CreateUserCommand {Email = "NOREALEMAIL", Password = "sadfasg"};
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, MimeTypes.Json)
            };

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Register_EmailAlreadyExists_ShouldReturn409()
        {
            // Arrange
            var user = await WolkDbContext.CreateAndSaveUser();

            var url = "/api/user";
            var command = new CreateUserCommand {Email = user.Email, Password = "sadfasg"};
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, MimeTypes.Json)
            };

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);
        }

        [TestMethod]
        public async Task Register_HappyFlow()
        {
            // Arrange
            var existingUser = await WolkDbContext.CreateAndSaveUser();

            var url = "/api/user";
            var command = new CreateUserCommand {Email = "mail@wolk.com", Password = "Passswordddd123!@#"};
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, MimeTypes.Json)
            };

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

            var user = await WolkDbContext.Users.SingleAsync(u => u.Email == command.Email);
            AssertCorrectPassword(user, command.Password);
        }

        protected override void BeforeConfigure(IDictionary<string, string> config) =>
            config.Add("WolkConfiguration:EnableUserRegistration", "true");
    }

    [TestClass]
    public class RegisterWithRegistrationTurnedOff : IntegrationTestBase
    {
        protected override void BeforeConfigure(IDictionary<string, string> config) =>
            config.Add("WolkConfiguration:EnableUserRegistration", "false");

        [TestMethod]
        public async Task Register_RegistrationTurnedOff_ShouldReturn400()
        {
            // Arrange
            var existingUser = await WolkDbContext.CreateAndSaveUser();

            var url = "/api/user";
            var command = new CreateUserCommand {Email = "mail@wolk.com", Password = "Passswordddd123!@#"};
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, MimeTypes.Json)
            };

            // Act
            using var response = await HttpClient.SendAsync(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(content.Contains("User registration not allowed according to condiguration."));
        }
    }
}
