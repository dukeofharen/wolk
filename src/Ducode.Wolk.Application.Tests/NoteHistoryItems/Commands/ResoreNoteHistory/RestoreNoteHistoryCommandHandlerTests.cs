using System;
using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.NoteHistoryItems.Commands.RestoreNoteHistory;
using Ducode.Wolk.Application.NoteHistoryItems.Notifications.SaveNoteHistory;
using Ducode.Wolk.Persistence;
using Ducode.Wolk.TestUtilities.Data;
using Ducode.Wolk.TestUtilities.FakeData;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Ducode.Wolk.Application.Tests.NoteHistoryItems.Commands.ResoreNoteHistory
{
    [TestClass]
    public class RestoreNoteHistoryCommandHandlerTests
    {
        private readonly Mock<IMediator> _mockMediator = new Mock<IMediator>();
        private readonly WolkDbContext _wolkDbContext = InMemoryDbContextFactory.Create();
        private RestoreNoteHistoryCommandHandler _handler;

        [TestInitialize]
        public void Setup() => _handler = new RestoreNoteHistoryCommandHandler(
            _mockMediator.Object,
            _wolkDbContext);

        [TestCleanup]
        public void Cleanup()
        {
            _mockMediator.VerifyAll();
            _wolkDbContext.Destroy();
        }

        [TestMethod]
        public async Task Handle_NoteNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var note = await _wolkDbContext.CreateAndSaveNote();
            var noteHistory = await _wolkDbContext.CreateAndSaveNoteHistory(note);
            var command = new RestoreNoteHistoryCommand {NoteId = note.Id + 1, NoteHistoryId = noteHistory.Id};

            // Act
            var exception = await Assert.ThrowsExceptionAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.IsTrue(exception.Message.Contains("'Note'"));
        }

        [TestMethod]
        public async Task Handle_NoteFound_NoteHistoryNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var note = await _wolkDbContext.CreateAndSaveNote();
            var noteHistory = await _wolkDbContext.CreateAndSaveNoteHistory(note);
            var command = new RestoreNoteHistoryCommand {NoteId = note.Id, NoteHistoryId = noteHistory.Id + 1};

            // Act
            var exception = await Assert.ThrowsExceptionAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.IsTrue(exception.Message.Contains("'NoteHistory'"));
        }

        [TestMethod]
        public async Task Handle_NoteFound_NoteHistoryFound_HistoryItemNotPartOfNote_ShouldThrowArgumentException()
        {
            // Arrange
            var note1 = await _wolkDbContext.CreateAndSaveNote();
            var note2 = await _wolkDbContext.CreateAndSaveNote();
            var noteHistory = await _wolkDbContext.CreateAndSaveNoteHistory(note1);
            var command = new RestoreNoteHistoryCommand {NoteId = note2.Id, NoteHistoryId = noteHistory.Id};

            // Act
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(() =>
                _handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.AreEqual(
                $"Note history with ID '{noteHistory.Id}' does not belong to note with ID '{note2.Id}'.",
                exception.Message);
        }

        [TestMethod]
        public async Task Handle_NoteFound_NoteHistoryFound_ShouldRestoreHistory()
        {
            // Arrange
            var note1 = await _wolkDbContext.CreateAndSaveNote();
            var note2 = await _wolkDbContext.CreateAndSaveNote();
            var noteHistory = await _wolkDbContext.CreateAndSaveNoteHistory(note1);
            var command = new RestoreNoteHistoryCommand {NoteId = note1.Id, NoteHistoryId = noteHistory.Id};

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.AreEqual(noteHistory.Title, note1.Title);
            Assert.AreEqual(noteHistory.Content, note1.Content);
            Assert.AreEqual(noteHistory.NoteType, note1.NoteType);

            _mockMediator.Verify(m => m.Publish(
                It.Is<SaveNoteHistoryNotification>(n => n.NoteId == note1.Id),
                It.IsAny<CancellationToken>()));
        }
    }
}
