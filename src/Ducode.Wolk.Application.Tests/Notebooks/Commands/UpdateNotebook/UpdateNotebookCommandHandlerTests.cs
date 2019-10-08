using System;
using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.Notebooks.Commands.UpdateNotebook;
using Ducode.Wolk.Persistence;
using Ducode.Wolk.TestUtilities.Data;
using Ducode.Wolk.TestUtilities.FakeData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ducode.Wolk.Application.Tests.Notebooks.Commands.UpdateNotebook
{
    [TestClass]
    public class UpdateNotebookCommandHandlerTests
    {
        private readonly WolkDbContext _wolkDbContext = InMemoryDbContextFactory.Create();
        private UpdateNotebookCommandHandler _handler;

        [TestInitialize]
        public void Setup() => _handler = new UpdateNotebookCommandHandler(_wolkDbContext);

        [TestCleanup]
        public void Cleanup() => InMemoryDbContextFactory.Destroy(_wolkDbContext);

        [TestMethod]
        public async Task Handle_NotebookNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var notebook = await _wolkDbContext.CreateAndSaveNotebook();
            var request = new UpdateNotebookCommand {Id = notebook.Id + 1, Name = Guid.NewGuid().ToString()};

            // Act / Assert
            await Assert.ThrowsExceptionAsync<NotFoundException>(() =>
                _handler.Handle(request, CancellationToken.None));
        }

        [TestMethod]
        public async Task Handle_NotebookFound_ShouldUpdate()
        {
            // Arrange
            var notebook = await _wolkDbContext.CreateAndSaveNotebook();
            var request = new UpdateNotebookCommand {Id = notebook.Id, Name = Guid.NewGuid().ToString()};

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.AreEqual(notebook.Name, request.Name);
        }
    }
}
