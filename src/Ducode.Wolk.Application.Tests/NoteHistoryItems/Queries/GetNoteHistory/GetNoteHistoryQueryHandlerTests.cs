using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.NoteHistoryItems.Queries.GetNoteHistory;
using Ducode.Wolk.Persistence;
using Ducode.Wolk.TestUtilities.Data;
using Ducode.Wolk.TestUtilities.FakeData;
using Ducode.Wolk.TestUtilities.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ducode.Wolk.Application.Tests.NoteHistoryItems.Queries.GetNoteHistory
{
    [TestClass]
    public class GetNoteHistoryQueryHandlerTests
    {
        private readonly IMapper _mapper = MapperFactory.GetMapper();
        private readonly WolkDbContext _wolkDbContext = InMemoryDbContextFactory.Create();
        private GetNoteHistoryQueryHandler _handler;

        [TestInitialize]
        public void Initialize() => _handler = new GetNoteHistoryQueryHandler(_wolkDbContext, _mapper);

        [TestCleanup]
        public void Cleanup() => _wolkDbContext.Destroy();

        [TestMethod]
        public async Task Handle_NoteNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var note = await _wolkDbContext.CreateAndSaveNote();
            var query = new GetNoteHistoryQuery {NoteId = note.Id + 1};

            // Act / Assert
            await Assert.ThrowsExceptionAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
        }

        [TestMethod]
        public async Task Handle_NoteFound_IncludeFullContent_ShouldSetAllContent()
        {
            // Arrange
            var note1 = await _wolkDbContext.CreateAndSaveNote();
            var note2 = await _wolkDbContext.CreateAndSaveNote();
            var hist1 = await _wolkDbContext.CreateAndSaveNoteHistory(note1);
            var hist2 = await _wolkDbContext.CreateAndSaveNoteHistory(note1);
            var hist3 = await _wolkDbContext.CreateAndSaveNoteHistory(note2);
            var query = new GetNoteHistoryQuery {NoteId = note1.Id, IncludeFullContent = true};

            // Act
            var result = (await _handler.Handle(query, CancellationToken.None)).ToArray();

            // Assert
            Assert.AreEqual(2, result.Length);
            Assert.IsTrue(result.All(h => h.Id == hist1.Id || h.Id == hist2.Id));
            Assert.IsTrue(result.All(h => !string.IsNullOrWhiteSpace(h.Preview)));
            Assert.IsTrue(result.All(h => !string.IsNullOrWhiteSpace(h.Content)));
        }

        [TestMethod]
        public async Task Handle_NoteFound_DontIncludeFullContent_ShouldSetNoContent()
        {
            // Arrange
            var note1 = await _wolkDbContext.CreateAndSaveNote();
            var note2 = await _wolkDbContext.CreateAndSaveNote();
            var hist1 = await _wolkDbContext.CreateAndSaveNoteHistory(note1);
            var hist2 = await _wolkDbContext.CreateAndSaveNoteHistory(note1);
            var hist3 = await _wolkDbContext.CreateAndSaveNoteHistory(note2);
            var query = new GetNoteHistoryQuery {NoteId = note1.Id, IncludeFullContent = false};

            // Act
            var result = (await _handler.Handle(query, CancellationToken.None)).ToArray();

            // Assert
            Assert.AreEqual(2, result.Length);
            Assert.IsTrue(result.All(h => h.Id == hist1.Id || h.Id == hist2.Id));
            Assert.IsTrue(result.All(h => !string.IsNullOrWhiteSpace(h.Preview)));
            Assert.IsFalse(result.Any(h => !string.IsNullOrWhiteSpace(h.Content)));
        }
    }
}
