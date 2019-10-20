using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ducode.Wolk.Application.Attachments.Queries.GetAttachments;
using Ducode.Wolk.Persistence;
using Ducode.Wolk.TestUtilities.Data;
using Ducode.Wolk.TestUtilities.FakeData;
using Ducode.Wolk.TestUtilities.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Ducode.Wolk.TestUtilities.Assertions.AttachmentAssertions;

namespace Ducode.Wolk.Application.Tests.Attachments.Queries.GetAttachments
{
    [TestClass]
    public class GetAttachmentsQueryHandlerTests
    {
        private readonly IMapper _mapper = MapperFactory.GetMapper();
        private readonly WolkDbContext _wolkDbContext = InMemoryDbContextFactory.Create();
        private GetAttachmentsQueryHandler _handler;

        [TestInitialize]
        public void Setup() => _handler = new GetAttachmentsQueryHandler(_mapper, _wolkDbContext);

        [TestCleanup]
        public void Cleanup() => _wolkDbContext.Destroy();

        [TestMethod]
        public async Task Handle_HappyFlow()
        {
            // Arrange
            var note1 = await _wolkDbContext.CreateAndSaveNote();
            var note2 = await _wolkDbContext.CreateAndSaveNote();

            var file1 = await _wolkDbContext.CreateAndSaveAttachment(note1);
            var file2 = await _wolkDbContext.CreateAndSaveAttachment(note2);
            var file3 = await _wolkDbContext.CreateAndSaveAttachment(note1);

            var request = new GetAttachmentsQuery {NoteId = note1.Id};

            // Act
            var result = (await _handler.Handle(request, CancellationToken.None)).ToArray();

            // Assert
            Assert.AreEqual(2, result.Length);
            ShouldBeEqual(file1, result[0]);
            ShouldBeEqual(file3, result[1]);
        }
    }
}
