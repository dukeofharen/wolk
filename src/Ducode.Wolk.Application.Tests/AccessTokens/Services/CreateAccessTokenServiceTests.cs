using System;
using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.AccessTokens.Services;
using Ducode.Wolk.Domain.Entities.Enums;
using Ducode.Wolk.Persistence;
using Ducode.Wolk.TestUtilities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ducode.Wolk.Application.Tests.AccessTokens.Services
{
    [TestClass]
    public class CreateAccessTokenServiceTests
    {
        private readonly WolkDbContext _wolkDbContext = InMemoryDbContextFactory.Create();
        private CreateAccessTokenService _service;

        [TestInitialize]
        public void Setup() => _service = new CreateAccessTokenService(_wolkDbContext);

        [TestCleanup]
        public void Cleanup() => _wolkDbContext.Destroy();

        [TestMethod]
        public async Task CreateAccessTokenAsync_ShouldCreateTokenSuccessfully()
        {
            // Arrange
            var identifier = 4;
            var type = AccessTokenType.Attachment;
            var expirationDateTime = new DateTimeOffset(2019, 12, 31, 23, 0, 0, TimeSpan.FromHours(2));

            // Act
            var result =
                await _service.CreateAccessTokenAsync(identifier, type, expirationDateTime, CancellationToken.None);

            // Assert
            var addedToken = await _wolkDbContext.AccessTokens.SingleAsync();

            Assert.IsTrue(Guid.TryParse(result.Token, out var _));
            Assert.AreEqual(identifier.ToString(), addedToken.Identifier);
            Assert.AreEqual(type, addedToken.AccessTokenType);
            Assert.AreEqual(expirationDateTime, addedToken.ExpirationDateTime);

            Assert.AreEqual(addedToken.Token, result.Token);
            Assert.AreEqual(addedToken.ExpirationDateTime, result.ExpirationDateTime);
        }
    }
}
