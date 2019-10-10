using System.Linq;
using Ducode.Wolk.Application.Notes.Commands.CreateNote;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ducode.Wolk.Application.Tests.Notes.Commands.CreateNote
{
    [TestClass]
    public class CreateNoteCommandValidatorTests
    {
        private readonly CreateNoteCommandValidator _validator = new CreateNoteCommandValidator();

        [TestMethod]
        public void Validate_ValidationErrors()
        {
            // Arrange
            var command = new CreateNoteCommand {Content = string.Empty, Title = new string('a', 201)};

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.AreEqual(2, result.Errors.Count);
            Assert.IsTrue(result.Errors.ElementAt(0).ErrorMessage.Contains("200 characters or fewer"));
            Assert.IsTrue(result.Errors.ElementAt(1).ErrorMessage.Contains("must not be empty"));
        }

        [TestMethod]
        public void Validate_NoValidationErrors()
        {
            // Arrange
            var command = new CreateNoteCommand {Content = "Note contents", Title = new string('a', 199)};

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.IsFalse(result.Errors.Any());
        }
    }
}
