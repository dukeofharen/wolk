using System.Linq;
using Ducode.Wolk.Application.Notebooks.Commands.UpdateNotebook;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ducode.Wolk.Application.Tests.Notebooks.Commands.UpdateNotebook
{
    [TestClass]
    public class UpdateNotebookCommandValidatorTests
    {
        private readonly UpdateNotebookCommandValidator _validator = new UpdateNotebookCommandValidator();

        [TestMethod]
        public void Validate_ValidationErrors()
        {
            // Arrange
            var command = new UpdateNotebookCommand {Id = 1, Name = new string('a', 201)};

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.IsTrue(result.Errors.Single().ErrorMessage.Contains("200 characters or fewer"));
        }

        [TestMethod]
        public void Validate_NoValidationErrors()
        {
            // Arrange
            var command = new UpdateNotebookCommand {Id = 1, Name = new string('a', 199)};

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.IsFalse(result.Errors.Any());
        }
    }
}
