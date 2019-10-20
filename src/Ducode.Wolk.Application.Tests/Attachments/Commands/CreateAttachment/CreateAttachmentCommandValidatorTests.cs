using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Attachments.Commands.CreateAttachment;
using Ducode.Wolk.Persistence;
using Ducode.Wolk.TestUtilities.Data;
using Ducode.Wolk.TestUtilities.FakeData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ducode.Wolk.Application.Tests.Attachments.Commands.CreateAttachment
{
    [TestClass]
    public class CreateAttachmentCommandValidatorTests
    {
        private readonly WolkDbContext _wolkDbContext = InMemoryDbContextFactory.Create();
        private CreateAttachmentCommandValidator _validator;

        [TestInitialize]
        public void Setup() => _validator = new CreateAttachmentCommandValidator(_wolkDbContext);

        [TestCleanup]
        public void Cleanup() => _wolkDbContext.Destroy();

        [TestMethod]
        public async Task Validate_ValidationErrors()
        {
            // Arrange
            var note = await _wolkDbContext.CreateAndSaveNote();
            var request = new CreateAttachmentCommand
            {
                Contents = new byte[0], Filename = new string('a', 301), NoteId = note.Id + 1
            };

            // Act
            var result = await _validator.ValidateAsync(request, CancellationToken.None);

            // Assert
            Assert.AreEqual(3, result.Errors.Count);
            Assert.IsTrue(result.Errors.ElementAt(0).ErrorMessage.Contains("Please provide a file."));
            Assert.IsTrue(result.Errors.ElementAt(1).ErrorMessage.Contains("300 characters or fewer"));
            Assert.IsTrue(result.Errors.ElementAt(2).ErrorMessage.Contains("Note with ID"));
        }

        [TestMethod]
        public async Task Validate_NoValidationErrors()
        {
            // Arrange
            var note = await _wolkDbContext.CreateAndSaveNote();
            var request = new CreateAttachmentCommand
            {
                Contents = new byte[] {1, 2, 3}, Filename = new string('a', 299), NoteId = note.Id
            };

            // Act
            var result = await _validator.ValidateAsync(request, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.Errors.Any());
        }
    }
}
