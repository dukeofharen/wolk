using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.Notes.Queries.GetNote;
using Ducode.Wolk.Common;
using Ducode.Wolk.Persistence;
using Ducode.Wolk.TestUtilities.Data;
using Ducode.Wolk.TestUtilities.FakeData;
using Ducode.Wolk.TestUtilities.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using static Ducode.Wolk.TestUtilities.Assertions.NoteAssertions;

namespace Ducode.Wolk.Application.Tests.Notes.Queries.GetNote
{
    [TestClass]
    public class GetNoteQueryHandlerTests
    {
        private readonly Mock<IDateTime> _mockDateTime = new Mock<IDateTime>();
        private readonly IMapper _mapper = MapperFactory.GetMapper();
        private readonly WolkDbContext _wolkDbContext = InMemoryDbContextFactory.Create();
        private GetNoteQueryHandler _handler;

        [TestInitialize]
        public void Setup() => _handler = new GetNoteQueryHandler(_mockDateTime.Object, _mapper, _wolkDbContext);

        [TestCleanup]
        public void Cleanup()
        {
            _mockDateTime.VerifyAll();
            _wolkDbContext.Destroy();
        }

        [TestMethod]
        public async Task Handle_NoteNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var note = await _wolkDbContext.CreateAndSaveNote();
            var request = new GetNoteQuery(note.Id + 1);

            // Act / Assert
            await Assert.ThrowsExceptionAsync<NotFoundException>(() =>
                _handler.Handle(request, CancellationToken.None));
        }

        [TestMethod]
        public async Task Handle_NoteFound_OpenedUpdated()
        {
            // Arrange
            var now = DateTime.Now;
            _mockDateTime
                .Setup(m => m.Now)
                .Returns(now);
            var note = await _wolkDbContext.CreateAndSaveNote();
            var request = new GetNoteQuery(note.Id) {UpdateOpenedDateTime = true};

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ShouldBeEqual(note, result);
            Assert.AreEqual(now, result.Opened);
        }

        [TestMethod]
        public async Task Handle_NoteFound_OpenedNotUpdated()
        {
            // Arrange
            var note = await _wolkDbContext.CreateAndSaveNote();
            var request = new GetNoteQuery(note.Id) {UpdateOpenedDateTime = false};

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            ShouldBeEqual(note, result);
            Assert.IsNull(result.Opened);
        }
    }
}
