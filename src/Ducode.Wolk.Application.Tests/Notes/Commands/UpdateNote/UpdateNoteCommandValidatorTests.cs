using System.Linq;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Notes.Commands.CreateNote;
using Ducode.Wolk.Application.Notes.Commands.UpdateNote;
using Ducode.Wolk.Persistence;
using Ducode.Wolk.TestUtilities.Data;
using Ducode.Wolk.TestUtilities.FakeData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ducode.Wolk.Application.Tests.Notes.Commands.UpdateNote
{
    [TestClass]
    public class UpdateNoteCommandValidatorTests
    {
        private readonly WolkDbContext _wolkDbContext = InMemoryDbContextFactory.Create();
        private UpdateNoteCommandValidator _validator;

        [TestInitialize]
        public void Setup() => _validator = new UpdateNoteCommandValidator(_wolkDbContext);

        [TestCleanup]
        public void Cleanup() => _wolkDbContext.Destroy();

        [TestMethod]
        public async Task Validate_ValidationErrors()
        {
            // Arrange
            var notebook = await _wolkDbContext.CreateAndSaveNotebook();
            var command = new UpdateNoteCommand
            {
                Content = string.Empty, Title = new string('a', 201), NotebookId = notebook.Id + 1
            };

            // Act
            var result = await _validator.ValidateAsync(command);

            // Assert
            Assert.AreEqual(3, result.Errors.Count);
            Assert.IsTrue(result.Errors.ElementAt(0).ErrorMessage.Contains("200 characters or fewer"));
            Assert.IsTrue(result.Errors.ElementAt(1).ErrorMessage.Contains("must not be empty"));
            Assert.IsTrue(result.Errors.ElementAt(2).ErrorMessage.Contains("Notebook with ID"));
        }

        [TestMethod]
        public async Task Validate_NoValidationErrors()
        {
            // Arrange
            var notebook = await _wolkDbContext.CreateAndSaveNotebook();
            var command = new UpdateNoteCommand
            {
                Content = "Note contents", Title = new string('a', 199), NotebookId = notebook.Id
            };

            // Act
            var result = await _validator.ValidateAsync(command);

            // Assert
            Assert.IsFalse(result.Errors.Any());
        }
    }
}
