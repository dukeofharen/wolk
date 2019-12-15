using System.Threading.Tasks;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Domain.Entities;
using Ducode.Wolk.Identity.Impl;
using Ducode.Wolk.Identity.Interfaces;
using Ducode.Wolk.Persistence;
using Ducode.Wolk.TestUtilities.Data;
using Ducode.Wolk.TestUtilities.FakeData;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Ducode.Wolk.Identity.Tests
{
    [TestClass]
    public class SignInMangerTests
    {
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
        private readonly Mock<IUserManager> _mockUserManager = new Mock<IUserManager>();
        private readonly WolkDbContext _wolkDbContext = InMemoryDbContextFactory.Create();
        private SignInManager _manager;

        [TestInitialize]
        public void Setup() => _manager = new SignInManager(_passwordHasher, _mockUserManager.Object, _wolkDbContext);

        [TestCleanup]
        public void Cleanup() => _wolkDbContext.Destroy();

        [TestMethod]
        public async Task CheckCredentialsAsync_UserNotFound_ShouldThrowUnauthorizedException()
        {
            // Arrange
            var user = await _wolkDbContext.CreateAndSaveUser();

            // Act / Assert
            await Assert.ThrowsExceptionAsync<UnauthorizedException>(() =>
                _manager.CheckCredentialsAsync(user.Email + "nl", "pass"));
        }

        [TestMethod]
        public async Task CheckCredentialsAsync_Success_ShouldReturnTrue()
        {
            // Arrange
            var user = await _wolkDbContext.CreateAndSaveUser();

            // Act
            var result = await _manager.CheckCredentialsAsync(user.Email, "Pass123");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task CheckCredentialsAsync_WrongPassword_ShouldReturnFalse()
        {
            // Arrange
            var user = await _wolkDbContext.CreateAndSaveUser();

            // Act
            var result = await _manager.CheckCredentialsAsync(user.Email, "Pass122");

            // Assert
            Assert.IsFalse(result);
        }
    }
}
