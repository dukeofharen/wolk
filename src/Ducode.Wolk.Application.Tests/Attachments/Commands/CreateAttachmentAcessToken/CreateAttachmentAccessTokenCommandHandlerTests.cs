using System;
using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.AccessTokens.Models;
using Ducode.Wolk.Application.AccessTokens.Services;
using Ducode.Wolk.Application.Attachments.Commands.CreateAttachmentAccessToken;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Domain.Entities.Enums;
using Ducode.Wolk.Persistence;
using Ducode.Wolk.TestUtilities.Data;
using Ducode.Wolk.TestUtilities.FakeData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Ducode.Wolk.Application.Tests.Attachments.Commands.CreateAttachmentAcessToken
{
    [TestClass]
    public class CreateAttachmentAccessTokenCommandHandlerTests
    {
        private readonly Mock<ICreateAccessTokenService> _mockCreateAccessTokenService =
            new Mock<ICreateAccessTokenService>();

        private readonly WolkDbContext _wolkDbContext = InMemoryDbContextFactory.Create();
        private CreateAttachmentAccessTokenCommandHandler _handler;

        [TestInitialize]
        public void Setup() => _handler =
            new CreateAttachmentAccessTokenCommandHandler(_mockCreateAccessTokenService.Object, _wolkDbContext);

        [TestCleanup]
        public void Cleanup()
        {
            _mockCreateAccessTokenService.VerifyAll();
            _wolkDbContext.Destroy();
        }

        [TestMethod]
        public async Task Handle_AttachmentNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var attachment = await _wolkDbContext.CreateAndSaveAttachment();
            var request = new CreateAttachmentAccessTokenCommand {AttachmentId = attachment.Id + 1};

            // Act / Assert
            await Assert.ThrowsExceptionAsync<NotFoundException>(() =>
                _handler.Handle(request, CancellationToken.None));
        }

        [TestMethod]
        public async Task Handle_ShouldAddAccessTokenCorrectly()
        {
            // Arrange
            var attachment = await _wolkDbContext.CreateAndSaveAttachment();
            var expirationDateTime = new DateTimeOffset(2019, 12, 31, 23, 0, 0, TimeSpan.FromHours(2));
            var request = new CreateAttachmentAccessTokenCommand
            {
                AttachmentId = attachment.Id,
                ExpirationDateTime = expirationDateTime
            };

            var expectedResult = new AccessTokenResultDto
            {
                ExpirationDateTime = expirationDateTime, Token = Guid.NewGuid().ToString()
            };
            _mockCreateAccessTokenService
                .Setup(m => m.CreateAccessTokenAsync(
                    attachment.Id,
                    AccessTokenType.Attachment,
                    expirationDateTime,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(attachment.Filename, result.Filename);
        }
    }
}
