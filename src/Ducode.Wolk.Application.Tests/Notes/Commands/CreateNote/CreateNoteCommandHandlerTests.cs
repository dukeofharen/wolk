using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.Notes.Commands.CreateNote;
using Ducode.Wolk.Persistence;
using Ducode.Wolk.TestUtilities.Data;
using Ducode.Wolk.TestUtilities.FakeData;
using Ducode.Wolk.TestUtilities.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Ducode.Wolk.TestUtilities.Assertions.NoteAssertions;

namespace Ducode.Wolk.Application.Tests.Notes.Commands.CreateNote
{
    [TestClass]
    public class CreateNoteCommandHandlerTests
    {
        private readonly IMapper _mapper = MapperFactory.GetMapper();
        private readonly WolkDbContext _wolkDbContext = InMemoryDbContextFactory.Create();
        private CreateNoteCommandHandler _handler;

        [TestInitialize]
        public void Setup() => _handler = new CreateNoteCommandHandler(_mapper, _wolkDbContext);

        [TestCleanup]
        public void Cleanup() => _wolkDbContext.Destroy();

        [TestMethod]
        public async Task Handle_ShouldAddNoteCorrectly()
        {
            // Arrange
            var notebook = await _wolkDbContext.CreateAndSaveNotebook();

            var request = new CreateNoteCommand
            {
                Content = "bladibla", Title = "Note title", NotebookId = notebook.Id
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            var note = await _wolkDbContext.Notes.SingleAsync();
            ShouldBeEqual(note, result);
        }
    }
}
