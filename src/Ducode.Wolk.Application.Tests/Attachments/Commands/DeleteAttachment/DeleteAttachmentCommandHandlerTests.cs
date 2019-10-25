using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Attachments.Commands.DeleteAttachment;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Common;
using Ducode.Wolk.Configuration;
using Ducode.Wolk.Persistence;
using Ducode.Wolk.TestUtilities.Config;
using Ducode.Wolk.TestUtilities.Data;
using Ducode.Wolk.TestUtilities.FakeData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Ducode.Wolk.Application.Tests.Attachments.Commands.DeleteAttachment
{
    [TestClass]
    public class DeleteAttachmentCommandHandlerTests
    {
        private readonly Mock<IFileService> _mockFileService = new Mock<IFileService>();
        private readonly WolkDbContext _wolkDbContext = InMemoryDbContextFactory.Create();
        private readonly WolkConfiguration _configuration = new WolkConfiguration {UploadsPath = "/srv/uploads"};
        private DeleteAttachmentCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            var mockOptions = MockOptions<WolkConfiguration>.Create(_configuration);
            _handler = new DeleteAttachmentCommandHandler(
                _mockFileService.Object,
                _wolkDbContext,
                mockOptions);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _mockFileService.VerifyAll();
            _wolkDbContext.Destroy();
        }

        [TestMethod]
        public async Task Handle_AttachmentNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var attachment = await _wolkDbContext.CreateAndSaveAttachment();
            var request = new DeleteAttachmentCommand {AttachmentId = attachment.Id + 1};

            // Act / Assert
            await Assert.ThrowsExceptionAsync<NotFoundException>(() =>
                _handler.Handle(request, CancellationToken.None));
        }

        [TestMethod]
        public async Task Handle_AttachmentFound_FileNotFound_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var attachment = await _wolkDbContext.CreateAndSaveAttachment();
            var request = new DeleteAttachmentCommand {AttachmentId = attachment.Id};

            var path = Path.Combine(_configuration.UploadsPath, attachment.InternalFilename);
            _mockFileService
                .Setup(m => m.FileExists(path))
                .Returns(false);

            // Act / Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                _handler.Handle(request, CancellationToken.None));
        }

        [TestMethod]
        public async Task Handle_AttachmentFound_FileFound_ShouldDeleteAttachmentAndFile()
        {
            // Arrange
            var attachment = await _wolkDbContext.CreateAndSaveAttachment();
            var request = new DeleteAttachmentCommand {AttachmentId = attachment.Id};

            var path = Path.Combine(_configuration.UploadsPath, attachment.InternalFilename);
            _mockFileService
                .Setup(m => m.FileExists(path))
                .Returns(true);

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsFalse(await _wolkDbContext.Attachments.AnyAsync());
            _mockFileService
                .Verify(m => m.DeleteFile(path), Times.Once);
        }
    }
}
