using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.Notebooks.Commands.UpdateNotebook;
using Ducode.Wolk.Application.Notes.Commands.CreateNote;
using Ducode.Wolk.Application.Notes.Commands.UpdateNote;
using Ducode.Wolk.Persistence;
using Ducode.Wolk.TestUtilities.Data;
using Ducode.Wolk.TestUtilities.FakeData;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Ducode.Wolk.TestUtilities.Assertions.NoteAssertsions;

namespace Ducode.Wolk.Application.Tests.Notes.Commands.UpdateNote
{
    [TestClass]
    public class UpdateNoteCommandHandlerTests
    {
        private readonly WolkDbContext _wolkDbContext = InMemoryDbContextFactory.Create();
        private UpdateNoteCommandHandler _handler;

        [TestInitialize]
        public void Setup() => _handler = new UpdateNoteCommandHandler(_wolkDbContext);

        [TestCleanup]
        public void Cleanup() => _wolkDbContext.Destroy();

        [TestMethod]
        public async Task Handle_NotebookNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var notebook = await _wolkDbContext.CreateAndSaveNotebook();
            var note = await _wolkDbContext.CreateAndSaveNote(notebook);

            var request = new UpdateNoteCommand
            {
                Id = note.Id, Content = "bladibla", Title = "Note title", NotebookId = notebook.Id + 1
            };

            // Act / Assert
            await Assert.ThrowsExceptionAsync<NotFoundException>(() =>
                _handler.Handle(request, CancellationToken.None));
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
        public async Task Handle_ShouldUpdateNoteCorrectly()
        {
            // Arrange
            var notebook = await _wolkDbContext.CreateAndSaveNotebook();
            var note = await _wolkDbContext.CreateAndSaveNote();

            var request = new UpdateNoteCommand {Id = note.Id, Content = "bladibla", Title = "Note title", NotebookId = notebook.Id};

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            ShouldBeEqual(note, request);
        }
    }
}
