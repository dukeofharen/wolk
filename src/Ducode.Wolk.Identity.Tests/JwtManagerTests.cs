using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using Ducode.Wolk.Configuration;
using Ducode.Wolk.Identity.Impl;
using Ducode.Wolk.TestUtilities.FakeData;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Ducode.Wolk.Identity.Tests
{
    [TestClass]
    public class JwtManagerTests
    {
        private readonly Mock<IOptions<IdentityConfiguration>> _mockOptions =
            new Mock<IOptions<IdentityConfiguration>>();

        private readonly IdentityConfiguration _configuration = new IdentityConfiguration
        {
            JwtSecret = "25qwrgsdfsdfethdfgdsa", ExpirationInSeconds = 10
        };

        private JwtManager _manager;

        [TestInitialize]
        public void Setup()
        {
            _mockOptions.Setup(m => m.Value).Returns(_configuration);
            _manager = new JwtManager(_mockOptions.Object);
        }

        [TestCleanup]
        public void Cleanup() => _mockOptions.VerifyAll();

        [TestMethod]
        public void CreateJwt_ShouldCreateJwtSuccessfully()
        {
            // Arrange
            var user = UserFakeDataGenerator.CreateUser();
            user.Id = 2;

            // Act
            var jwt = _manager.CreateJwt(user);

            // Assert
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            var result = new JwtSecurityTokenHandler().ValidateToken(jwt,
                new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.JwtSecret)),
                    ValidateAudience = false,
                    ValidateIssuer = false
                }, out var validatedToken);

            Assert.AreEqual(user.Id.ToString(), result.Claims.Single(c => c.Type == "sub").Value);
        }
    }
}
