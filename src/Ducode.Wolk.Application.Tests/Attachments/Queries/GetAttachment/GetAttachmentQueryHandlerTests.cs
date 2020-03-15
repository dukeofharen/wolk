using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ducode.Wolk.Application.Attachments.Queries.GetAttachmentBinary;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Common;
using Ducode.Wolk.Configuration;
using Ducode.Wolk.Domain.Entities;
using Ducode.Wolk.Persistence;
using Ducode.Wolk.TestUtilities.Data;
using Ducode.Wolk.TestUtilities.FakeData;
using Ducode.Wolk.TestUtilities.Mapping;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using static Ducode.Wolk.TestUtilities.Assertions.AttachmentAssertions;

namespace Ducode.Wolk.Application.Tests.Attachments.Queries.GetAttachment
{
    [TestClass]
    public class GetAttachmentQueryHandlerTests
    {
        private readonly Mock<IDateTime> _mockDateTime = new Mock<IDateTime>();
        private readonly Mock<IFileService> _mockFileService = new Mock<IFileService>();
        private readonly IMapper _mapper = MapperFactory.GetMapper();
        private readonly WolkDbContext _wolkDbContext = InMemoryDbContextFactory.Create();
        private readonly WolkConfiguration _configuration = new WolkConfiguration {UploadsPath = "/srv/uploads"};
        private GetAttachmentQueryHandler _handler;

        [TestInitialize]
        public void Initialize()
        {
            var mockOptions = Options.Create(_configuration);
            _handler = new GetAttachmentQueryHandler(
                _mockDateTime.Object,
                _mockFileService.Object,
                _mapper,
                _wolkDbContext,
                mockOptions);

            _mockDateTime
                .Setup(m => m.Now)
                .Returns(DateTimeOffset.Now);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _mockFileService.VerifyAll();
            _wolkDbContext.Destroy();
        }

        [TestMethod]
        public async Task Handle_NoIdentifierSet_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var request = new GetAttachmentQuery {Token = null, AttachmentId = null};

            // Act / Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                _handler.Handle(request, CancellationToken.None));
        }

        [TestMethod]
        public async Task Handle_ById_AttachmentNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var attachment = await _wolkDbContext.CreateAndSaveAttachment();
            var request = new GetAttachmentQuery {AttachmentId = attachment.Id + 1};

            // Act
            var exception = await Assert.ThrowsExceptionAsync<NotFoundException>(() =>
                _handler.Handle(request, CancellationToken.None));

            // Assert
            Assert.IsTrue(exception.Message.Contains(nameof(Attachment)));
        }

        [TestMethod]
        public async Task Handle_ById_AttachmentFound_FileNotFound_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var attachment = await _wolkDbContext.CreateAndSaveAttachment();
            var request = new GetAttachmentQuery {AttachmentId = attachment.Id};

            var filePath = Path.Combine(_configuration.UploadsPath, attachment.InternalFilename);
            _mockFileService
                .Setup(m => m.FileExists(filePath))
                .Returns(false);

            // Act / Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                _handler.Handle(request, CancellationToken.None));
        }

        [TestMethod]
        public async Task Handle_ById_AttachmentFound_FileFound_ShouldReturnAttachmentCorrectly()
        {
            // Arrange
            var attachment = await _wolkDbContext.CreateAndSaveAttachment();
            var request = new GetAttachmentQuery {AttachmentId = attachment.Id};

            var filePath = Path.Combine(_configuration.UploadsPath, attachment.InternalFilename);
            _mockFileService
                .Setup(m => m.FileExists(filePath))
                .Returns(true);

            var fileContents = new byte[] {1, 2, 3, 4};
            _mockFileService
                .Setup(m => m.ReadAllBytes(filePath))
                .Returns(fileContents);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ShouldBeEqual(attachment, result);
            Assert.AreEqual(fileContents, result.Contents);
        }

        [TestMethod]
        public async Task Handle_ByToken_TokenNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var attachment = await _wolkDbContext.CreateAndSaveAttachment();
            var accessToken = await _wolkDbContext.CreateAndSaveAttachmentAccessToken(attachment);
            var request = new GetAttachmentQuery {Token = accessToken.Token + "1"};

            // Act
            var exception =
                await Assert.ThrowsExceptionAsync<NotFoundException>(() =>
                    _handler.Handle(request, CancellationToken.None));

            // Assert
            Assert.IsTrue(exception.Message.Contains(nameof(AccessToken)));
            Assert.IsTrue(exception.Message.Contains(accessToken.Token));
        }

        [TestMethod]
        public async Task Handle_ByToken_TokenFoundButExpired_ShouldThrowNotFoundException()
        {
            // Arrange
            var attachment = await _wolkDbContext.CreateAndSaveAttachment();
            var expirationDateTime = DateTimeOffset.Now.AddSeconds(-1);
            var accessToken = await _wolkDbContext.CreateAndSaveAttachmentAccessToken(attachment, expirationDateTime);
            var request = new GetAttachmentQuery {Token = accessToken.Token};

            // Act
            var exception =
                await Assert.ThrowsExceptionAsync<NotFoundException>(() =>
                    _handler.Handle(request, CancellationToken.None));

            // Assert
            Assert.AreEqual("The access token has expired.", exception.Message);
        }

        [TestMethod]
        public async Task Handle_ByToken_TokenFoundButIdentifierNotValid_ShouldThrowNotFoundException()
        {
            // Arrange
            var expirationDateTime = DateTimeOffset.Now.AddSeconds(5);
            var accessToken = await _wolkDbContext.CreateAndSaveAttachmentAccessToken("ID1", expirationDateTime);
            var request = new GetAttachmentQuery {Token = accessToken.Token};

            // Act
            var exception =
                await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                    _handler.Handle(request, CancellationToken.None));

            // Assert
            Assert.IsTrue(exception.Message.Contains("is not a valid number"));
        }

        [TestMethod]
        public async Task Handle_ByToken_TokenFound_ShouldReturnAttachmentSuccessfully()
        {
            // Arrange
            var attachment = await _wolkDbContext.CreateAndSaveAttachment();
            var expirationDateTime = DateTimeOffset.Now.AddSeconds(5);
            var accessToken = await _wolkDbContext.CreateAndSaveAttachmentAccessToken(attachment, expirationDateTime);
            var request = new GetAttachmentQuery {Token = accessToken.Token};

            var filePath = Path.Combine(_configuration.UploadsPath, attachment.InternalFilename);
            _mockFileService
                .Setup(m => m.FileExists(filePath))
                .Returns(true);

            var fileContents = new byte[] {1, 2, 3, 4};
            _mockFileService
                .Setup(m => m.ReadAllBytes(filePath))
                .Returns(fileContents);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ShouldBeEqual(attachment, result);
            Assert.AreEqual(fileContents, result.Contents);
        }
    }
}
