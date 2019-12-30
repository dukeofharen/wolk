using System.Linq;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Notes.Commands.CreateNote;
using Ducode.Wolk.Domain.Entities.Enums;
using Ducode.Wolk.Persistence;
using Ducode.Wolk.TestUtilities.Data;
using Ducode.Wolk.TestUtilities.FakeData;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ducode.Wolk.Application.Tests.Notes.Commands.CreateNote
{
    [TestClass]
    public class CreateNoteCommandValidatorTests
    {
        private readonly WolkDbContext _wolkDbContext = InMemoryDbContextFactory.Create();
        private CreateNoteCommandValidator _validator;

        [TestInitialize]
        public void Setup() => _validator = new CreateNoteCommandValidator(_wolkDbContext);

        [TestCleanup]
        public void Cleanup() => _wolkDbContext.Destroy();

        [TestMethod]
        public async Task Validate_ValidationErrors()
        {
            // Arrange
            var notebook = await _wolkDbContext.CreateAndSaveNotebook();
            var command = new CreateNoteCommand
            {
                Content = string.Empty,
                Title = new string('a', 201),
                NotebookId = notebook.Id + 1,
                NoteType = NoteType.NotSet
            };

            // Act
            var result = await _validator.ValidateAsync(command);

            // Assert
            Assert.AreEqual(3, result.Errors.Count);
            Assert.IsTrue(result.Errors.ElementAt(0).ErrorMessage.Contains("200 characters or fewer"));
            Assert.IsTrue(result.Errors.ElementAt(1).ErrorMessage.Contains("must not be equal to"));
            Assert.IsTrue(result.Errors.ElementAt(2).ErrorMessage.Contains("Notebook with ID"));
        }

        [TestMethod]
        public async Task Validate_NoValidationErrors()
        {
            // Arrange
            var notebook = await _wolkDbContext.CreateAndSaveNotebook();
            var command = new CreateNoteCommand
            {
                Content = "Note contents",
                Title = new string('a', 199),
                NotebookId = notebook.Id,
                NoteType = NoteType.PlainText
            };

            // Act
            var result = await _validator.ValidateAsync(command);

            // Assert
            Assert.IsFalse(result.Errors.Any());
        }
    }
}
