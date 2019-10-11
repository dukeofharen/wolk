using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ducode.Wolk.Application.Notebooks.Commands.CreateNotebook;
using Ducode.Wolk.Persistence;
using Ducode.Wolk.TestUtilities.Data;
using Ducode.Wolk.TestUtilities.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Ducode.Wolk.TestUtilities.Assertions.NotebookAssertions;

namespace Ducode.Wolk.Application.Tests.Notebooks.Commands.CreateNotebook
{
    [TestClass]
    public class CreateNotebookCommandHandlerTests
    {
        private readonly IMapper _mapper = MapperFactory.GetMapper();
        private readonly WolkDbContext _wolkDbContext = InMemoryDbContextFactory.Create();
        private CreateNotebookCommandHandler _handler;

        [TestInitialize]
        public void Setup() => _handler = new CreateNotebookCommandHandler(_mapper, _wolkDbContext);

        [TestCleanup]
        public void Cleanup() => _wolkDbContext.Destroy();

        [TestMethod]
        public async Task Handle_NotebookShouldBeAdded()
        {
            // Arrange
            var request = new CreateNotebookCommand {Name = "Notebook 01"};

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            var notebook = await _wolkDbContext.Notebooks.SingleAsync(n => n.Id == result.Id);
            ShouldBeEqual(notebook, result);
        }
    }
}
