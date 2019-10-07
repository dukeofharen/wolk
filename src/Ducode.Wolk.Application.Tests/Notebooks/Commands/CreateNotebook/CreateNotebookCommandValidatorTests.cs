using System.Linq;
using Ducode.Wolk.Application.Notebooks.Commands.CreateNotebook;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ducode.Wolk.Application.Tests.Notebooks.Commands.CreateNotebook
{
    [TestClass]
    public class CreateNotebookCommandValidatorTests
    {
        private readonly CreateNotebookCommandValidator _validator = new CreateNotebookCommandValidator();

        [TestMethod]
        public void Validate_ValidationErrors()
        {
            // Arrange
            var command = new CreateNotebookCommand {Name = new string('a', 201)};

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.IsTrue(result.Errors.Single().ErrorMessage.Contains("200 characters or fewer"));
        }
    }
}
