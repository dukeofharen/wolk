using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ducode.Wolk.Application.Attachments.Commands.CreateAttachment;
using Ducode.Wolk.Common;
using Ducode.Wolk.Configuration;
using Ducode.Wolk.Persistence;
using Ducode.Wolk.TestUtilities.Config;
using Ducode.Wolk.TestUtilities.Data;
using Ducode.Wolk.TestUtilities.FakeData;
using Ducode.Wolk.TestUtilities.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using static Ducode.Wolk.TestUtilities.Assertions.AttachmentAssertions;

namespace Ducode.Wolk.Application.Tests.Attachments.Commands.CreateAttachment
{
    [TestClass]
    public class CreateAttachmentCommandHandlerTests
    {
        private readonly Mock<IFileService> _mockFileService = new Mock<IFileService>();
        private readonly IMapper _mapper = MapperFactory.GetMapper();
        private readonly Mock<IMimeService> _mockMimeService = new Mock<IMimeService>();
        private readonly WolkDbContext _wolkDbContext = InMemoryDbContextFactory.Create();
        private readonly WolkConfiguration _configuration = new WolkConfiguration {UploadsPath = "/srv/wolk"};
        private CreateAttachmentCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            var mockOptions = MockOptions<WolkConfiguration>.Create(_configuration);
            _handler = new CreateAttachmentCommandHandler(
                _mockFileService.Object,
                _mapper,
                _mockMimeService.Object,
                _wolkDbContext,
                mockOptions);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _mockFileService.VerifyAll();
            _mockMimeService.VerifyAll();
            _wolkDbContext.Destroy();
        }

        [TestMethod]
        public async Task Handle_ShouldAddAttachmentCorrectly()
        {
            // Arrange
            var note = await _wolkDbContext.CreateAndSaveNote();
            var request = new CreateAttachmentCommand
            {
                Contents = new byte[]{1,2,3,4},
                Filename = "file.txt",
                NoteId = note.Id
            };

            var mime = "text/plain";
            _mockMimeService
                .Setup(m => m.GetMimeType(request.Filename))
                .Returns(mime);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.AreEqual(request.Filename, result.Filename);
            Assert.AreEqual(request.NoteId, result.NoteId);
            Assert.AreEqual(4, result.FileSize);
            Assert.AreEqual(mime, result.MimeType);

            var attachment = await _wolkDbContext.Attachments.SingleAsync();
            ShouldBeEqual(attachment, result);
            Assert.IsFalse(string.IsNullOrWhiteSpace(attachment.InternalFilename));

            var expectedFilePath = Path.Combine(_configuration.UploadsPath, attachment.InternalFilename);
            _mockFileService
                .Verify(m => m.WriteAllBytes(expectedFilePath, request.Contents), Times.Once);
        }
    }
}
