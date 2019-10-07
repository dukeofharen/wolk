using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.Notebooks.Commands.DeleteNotebook;
using Ducode.Wolk.Persistence;
using Ducode.Wolk.TestUtilities.Data;
using Ducode.Wolk.TestUtilities.FakeData;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ducode.Wolk.Application.Tests.Notebooks.Commands.DeleteNotebook
{
    [TestClass]
    public class DeleteNotebookCommandHandlerTests
    {
        private readonly WolkDbContext _wolkDbContext = InMemoryDbContextFactory.Create();
        private DeleteNotebookCommandHandler _handler;

        [TestInitialize]
        public void Setup() => _handler = new DeleteNotebookCommandHandler(_wolkDbContext);

        [TestCleanup]
        public void Cleanup() => InMemoryDbContextFactory.Destroy(_wolkDbContext);

        [TestMethod]
        public async Task Handle_NotebookNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var notebook = await _wolkDbContext.CreateAndSaveNotebook();
            var request = new DeleteNotebookCommand(notebook.Id + 1);

            // Act / Assert
            await Assert.ThrowsExceptionAsync<NotFoundException>(() =>
                _handler.Handle(request, CancellationToken.None));
        }

        [TestMethod]
        public async Task Handle_NotebookFound_ShouldDeleteNotebook()
        {
            // Arrange
            var notebook = await _wolkDbContext.CreateAndSaveNotebook();
            var request = new DeleteNotebookCommand(notebook.Id);

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsFalse(await _wolkDbContext.Notebooks.AnyAsync(n => n.Id == request.Id));
        }
    }
}
