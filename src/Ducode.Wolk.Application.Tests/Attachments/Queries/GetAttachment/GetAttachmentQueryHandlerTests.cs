using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ducode.Wolk.Application.Attachments.Queries.GetAttachmentBinary;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Common;
using Ducode.Wolk.Configuration;
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
        private readonly Mock<IFileService> _mockFileService = new Mock<IFileService>();
        private readonly IMapper _mapper = MapperFactory.GetMapper();
        private readonly WolkDbContext _wolkDbContext = InMemoryDbContextFactory.Create();
        private readonly WolkConfiguration _configuration = new WolkConfiguration
        {
            UploadsPath = "/srv/uploads"
        };
        private GetAttachmentQueryHandler _handler;

        [TestInitialize]
        public void Initialize()
        {
            var mockOptions = new Mock<IOptions<WolkConfiguration>>();
            mockOptions
                .Setup(m => m.Value)
                .Returns(_configuration);
            _handler = new GetAttachmentQueryHandler(
                _mockFileService.Object,
                _mapper,
                _wolkDbContext,
                mockOptions.Object);
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
            var request = new GetAttachmentQuery {AttachmentId = attachment.Id + 1};

            // Act / Assert
            await Assert.ThrowsExceptionAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));
        }

        [TestMethod]
        public async Task Handle_AttachmentFound_FileNotFound_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var attachment = await _wolkDbContext.CreateAndSaveAttachment();
            var request = new GetAttachmentQuery {AttachmentId = attachment.Id};

            var filePath = Path.Combine(_configuration.UploadsPath, attachment.InternalFilename);
            _mockFileService
                .Setup(m => m.FileExists(filePath))
                .Returns(false);

            // Act / Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _handler.Handle(request, CancellationToken.None));
        }

        [TestMethod]
        public async Task Handle_AttachmentFound_FileFound_ShouldReturnAttachmentCorrectly()
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
    }
}
