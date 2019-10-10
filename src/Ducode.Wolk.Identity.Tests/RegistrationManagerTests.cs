using System.Linq;
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
    public class RegistrationManagerTests
    {
        private readonly Mock<IUserManager> _mockUserManager = new Mock<IUserManager>();
        private readonly WolkDbContext _wolkDbContext = InMemoryDbContextFactory.Create();
        private RegistrationManager _manager;

        [TestInitialize]
        public void Setup() => _manager = new RegistrationManager(_mockUserManager.Object, _wolkDbContext);

        [TestCleanup]
        public void Cleanup()
        {
            _mockUserManager.VerifyAll();
            InMemoryDbContextFactory.Destroy(_wolkDbContext);
        }

        [TestMethod]
        public async Task RegisterUserAsync_EmailAlreayExists_ShouldThrowConflictException()
        {
            // Arrange
            var user = await _wolkDbContext.CreateAndSaveUser();

            // Act / Assert
            await Assert.ThrowsExceptionAsync<ConflictException>(() => _manager.RegisterUserAsync(user.Email, "pass"));
        }

        [TestMethod]
        public async Task RegisterUserAsync_ValidationErrors_ShouldThrowValidationException()
        {
            // Arrange
            var email = "test@gmail.com";
            var password = "Pass123";
            var identityResult = IdentityResult.Failed(new IdentityError {Description = "ERROR!!!"});
            _mockUserManager
                .Setup(m => m.CreateAsync(It.IsAny<User>(), password))
                .ReturnsAsync(identityResult);

            // Act
            var exception =
                await Assert.ThrowsExceptionAsync<ValidationException>(
                    () => _manager.RegisterUserAsync(email, password));

            // Assert
            Assert.IsTrue(exception.Failures.Single().Contains("ERROR!!!"));
        }

        [TestMethod]
        public async Task RegisterUserAsync_ShouldCreateUserSuccessfully()
        {
            // Arrange
            var email = "test@gmail.com";
            var password = "Pass123";
            var identityResult = IdentityResult.Success;
            User addedUser = null;
            _mockUserManager
                .Setup(m => m.CreateAsync(It.IsAny<User>(), password))
                .Callback<User, string>((user, _) => addedUser = user)
                .ReturnsAsync(identityResult);

            // Act
            await _manager.RegisterUserAsync(email, password);

            // Assert
            Assert.AreEqual(email, addedUser.Email);
        }
    }
}
