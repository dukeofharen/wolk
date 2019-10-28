using System;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.Interfaces.Identity;
using Ducode.Wolk.Identity.Impl;
using Ducode.Wolk.Persistence;
using Ducode.Wolk.TestUtilities.Data;
using Ducode.Wolk.TestUtilities.FakeData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Ducode.Wolk.Identity.Tests
{
    [TestClass]
    public class TokenRenewalManagerTests
    {
        private readonly Mock<IJwtManager> _mockJwtManager = new Mock<IJwtManager>();
        private readonly WolkDbContext _wolkDbContext = InMemoryDbContextFactory.Create();
        private TokenRenewalManager _manager;

        [TestInitialize]
        public void Initialize() => _manager = new TokenRenewalManager(_mockJwtManager.Object, _wolkDbContext);

        [TestCleanup]
        public void Cleanup()
        {
            _mockJwtManager.VerifyAll();
            _wolkDbContext.Destroy();
        }

        [TestMethod]
        public async Task RenewTokenAsync_UserNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var user = await _wolkDbContext.CreateAndSaveUser();

            // Act / Assert
            await Assert.ThrowsExceptionAsync<NotFoundException>(() => _manager.RenewTokenAsync(user.Id + 1));
        }

        [TestMethod]
        public async Task RenewTokenAsync_UserFound_ShouldReturnNewToken()
        {
            // Arrange
            var user = await _wolkDbContext.CreateAndSaveUser();
            var token = Guid.NewGuid().ToString();
            _mockJwtManager
                .Setup(m => m.CreateJwt(user))
                .Returns(token);

            // Act
            var result = await _manager.RenewTokenAsync(user.Id);

            // Assert
            Assert.AreEqual(token, result);
        }
    }
}
