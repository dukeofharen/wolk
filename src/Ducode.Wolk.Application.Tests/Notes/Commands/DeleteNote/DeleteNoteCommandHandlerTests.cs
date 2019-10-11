using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.Notebooks.Commands.DeleteNotebook;
using Ducode.Wolk.Persistence;
using Ducode.Wolk.TestUtilities.Data;
using Ducode.Wolk.TestUtilities.FakeData;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ducode.Wolk.Application.Tests.Notes.Commands.DeleteNote
{
    [TestClass]
    public class DeleteNoteCommandHandlerTests
    {
        private readonly WolkDbContext _wolkDbContext = InMemoryDbContextFactory.Create();
        private DeleteNotebookCommandHandler _handler;

        [TestInitialize]
        public void Setup() => _handler = new DeleteNotebookCommandHandler(_wolkDbContext);

        [TestCleanup]
        public void Cleanup() => _wolkDbContext.Destroy();

        [TestMethod]
        public async Task Handle_NoteNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var note = await _wolkDbContext.CreateAndSaveNote();
            var request = new DeleteNotebookCommand(note.Id + 1);

            // Act / Assert
            await Assert.ThrowsExceptionAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));
        }

        [TestMethod]
        public async Task Handle_NoteFound_ShouldDeleteNote()
        {
            // Arrange
            var note = await _wolkDbContext.CreateAndSaveNote();
            var request = new DeleteNotebookCommand(note.Id);

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsFalse(await _wolkDbContext.Notes.AnyAsync());
        }
    }
}
