using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.NoteHistoryItems.Notifications.SaveNoteHistory;
using Ducode.Wolk.Application.Notes.Commands.UpdateNote;
using Ducode.Wolk.Persistence;
using Ducode.Wolk.TestUtilities.Data;
using Ducode.Wolk.TestUtilities.FakeData;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using static Ducode.Wolk.TestUtilities.Assertions.NoteAssertions;

namespace Ducode.Wolk.Application.Tests.Notes.Commands.UpdateNote
{
    [TestClass]
    public class UpdateNoteCommandHandlerTests
    {
        private readonly Mock<IMediator> _mockMediator = new Mock<IMediator>();
        private readonly WolkDbContext _wolkDbContext = InMemoryDbContextFactory.Create();
        private UpdateNoteCommandHandler _handler;

        [TestInitialize]
        public void Setup() => _handler = new UpdateNoteCommandHandler(
            _mockMediator.Object,
            _wolkDbContext);

        [TestCleanup]
        public void Cleanup()
        {
            _wolkDbContext.Destroy();
            _mockMediator.VerifyAll();
        }

        [TestMethod]
        public async Task Handle_NoteNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var notebook = await _wolkDbContext.CreateAndSaveNotebook();
            var note = await _wolkDbContext.CreateAndSaveNote(notebook);

            var request = new UpdateNoteCommand
            {
                Id = note.Id + 1, Content = "bladibla", Title = "Note title", NotebookId = notebook.Id
            };

            // Act / Assert
            await Assert.ThrowsExceptionAsync<NotFoundException>(() =>
                _handler.Handle(request, CancellationToken.None));
        }

        [TestMethod]
        public async Task Handle_ShouldUpdateNoteCorrectly_And_AddHistoryItem()
        {
            // Arrange
            var notebook = await _wolkDbContext.CreateAndSaveNotebook();
            var note = await _wolkDbContext.CreateAndSaveNote();

            var request = new UpdateNoteCommand
            {
                Id = note.Id, Content = "bladibla", Title = "Note title", NotebookId = notebook.Id
            };

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            ShouldBeEqual(note, request);
            _mockMediator.Verify(m => m.Publish(
                It.Is<SaveNoteHistoryNotification>(n => n.NoteId == note.Id),
                It.IsAny<CancellationToken>()));
        }

        // [TestMethod]
        // public async Task Handle_ShouldDeleteOldHistoryItems()
        // {
        //     // Arrange
        //     var notebook = await _wolkDbContext.CreateAndSaveNotebook();
        //     var note = await _wolkDbContext.CreateAndSaveNote();
        //     var originalTitle = note.Title;
        //     var originalContent = note.Content;
        //
        //     // Act
        //     for (var i = 0; i < 7; i++)
        //     {
        //         var request = new UpdateNoteCommand
        //         {
        //             Id = note.Id,
        //             Content = Guid.NewGuid().ToString(),
        //             Title = Guid.NewGuid().ToString(),
        //             NotebookId = notebook.Id
        //         };
        //         await _handler.Handle(request, CancellationToken.None);
        //     }
        //
        //     // Assert
        //     Assert.AreEqual(_configuration.NoteHistoryLength, note.NoteHistory.Count);
        //     Assert.IsFalse(note.NoteHistory.Any(h => h.Title == originalTitle || h.Content == originalContent));
        // }
    }
}
