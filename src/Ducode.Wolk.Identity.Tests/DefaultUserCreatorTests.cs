using System;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Interfaces.Identity;
using Ducode.Wolk.Configuration;
using Ducode.Wolk.Domain.Entities;
using Ducode.Wolk.Identity.Impl;
using Ducode.Wolk.Persistence;
using Ducode.Wolk.TestUtilities.Config;
using Ducode.Wolk.TestUtilities.Data;
using Ducode.Wolk.TestUtilities.FakeData;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Ducode.Wolk.Identity.Tests
{
    [TestClass]
    public class DefaultUserCreatorTests
    {
        private readonly Mock<IPasswordHasher<User>> _mockPasswordHasher = new Mock<IPasswordHasher<User>>();
        private readonly Mock<IRegistrationManager> _mockRegistrationManager = new Mock<IRegistrationManager>();
        private readonly WolkDbContext _wolkDbContext = InMemoryDbContextFactory.Create();
        private readonly WolkConfiguration _configuration = new WolkConfiguration();
        private DefaultUserCreator _creator;

        [TestInitialize]
        public void Setup()
        {
            var mockOptions = MockOptions<WolkConfiguration>.Create(_configuration);
            _creator = new DefaultUserCreator(
                new Mock<ILogger<DefaultUserCreator>>().Object,
                _mockPasswordHasher.Object,
                _mockRegistrationManager.Object,
                _wolkDbContext,
                mockOptions);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _mockPasswordHasher.VerifyAll();
            _mockRegistrationManager.VerifyAll();
            _wolkDbContext.Destroy();
        }

        [TestMethod]
        public async Task CreateOrUpdateDefaultUser_ConfigNotSet_ShouldNotCreateDefaultUser()
        {
            // Arrange
            _configuration.DefaultLoginEmail = string.Empty;
            _configuration.DefaultPassword = string.Empty;

            // Act
            await _creator.CreateOrUpdateDefaultUser();

            // Assert
            _mockRegistrationManager.Verify(
                m => m.RegisterUserAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task CreateOrUpdateDefaultUser_ConfigSet_UserDoesNotExist_ShouldCreateDefaultUser()
        {
            // Arrange
            _configuration.DefaultLoginEmail = "wolk@wo.lk";
            _configuration.DefaultPassword = "Pass123!";

            // Act
            await _creator.CreateOrUpdateDefaultUser();

            // Assert
            _mockRegistrationManager.Verify(
                m => m.RegisterUserAsync(_configuration.DefaultLoginEmail, _configuration.DefaultPassword),
                Times.Once);
        }

        [TestMethod]
        public async Task CreateOrUpdateDefaultUser_ConfigSet_UserExists_PasswordChanged_ShouldUpdateDefaultUser()
        {
            // Arrange
            var user = await _wolkDbContext.CreateAndSaveUser();

            _configuration.DefaultLoginEmail = user.Email;
            _configuration.DefaultPassword = "Pass123!";

            var hashedPassword = Guid.NewGuid().ToString();
            _mockPasswordHasher
                .Setup(m => m.HashPassword(user, _configuration.DefaultPassword))
                .Returns(hashedPassword);

            // Act
            await _creator.CreateOrUpdateDefaultUser();

            // Assert
            Assert.AreEqual(hashedPassword, user.PasswordHash);
            Assert.IsNotNull(user.SecurityStamp);
        }
    }
}
