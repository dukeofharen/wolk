using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.NoteHistoryItems.Notifications.SaveNoteHistory;
using Ducode.Wolk.Application.Notes.Commands.UpdateNote;
using Ducode.Wolk.Configuration;
using Ducode.Wolk.Persistence;
using Ducode.Wolk.TestUtilities.Data;
using Ducode.Wolk.TestUtilities.FakeData;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Ducode.Wolk.Application.Tests.NoteHistoryItems.Notifications.SaveNoteHistory
{
    [TestClass]
    public class SaveNoteHistoryNotificationHandlerTests
    {
        private readonly Mock<ILogger<SaveNoteHistoryNotificationHandler>> _mockLogger =
            new Mock<ILogger<SaveNoteHistoryNotificationHandler>>();

        private readonly WolkDbContext _wolkDbContext = InMemoryDbContextFactory.Create();
        private readonly WolkConfiguration _configuration = new WolkConfiguration {NoteHistoryLength = 5};
        private SaveNoteHistoryNotificationHandler _handler;

        [TestInitialize]
        public void Initialize() =>
            _handler = new SaveNoteHistoryNotificationHandler(
                _mockLogger.Object,
                _wolkDbContext,
                Options.Create(_configuration));

        [TestCleanup]
        public void Cleanup()
        {
            _mockLogger.VerifyAll();
            _wolkDbContext.Destroy();
        }

        [TestMethod]
        public async Task Handle_NoteNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var note = await _wolkDbContext.CreateAndSaveNote();
            var notification = new SaveNoteHistoryNotification {NoteId = note.Id + 1};

            // Act / Assert
            await Assert.ThrowsExceptionAsync<NotFoundException>(() =>
                _handler.Handle(notification, CancellationToken.None));
        }

        [TestMethod]
        public async Task Handle_ShouldDeleteOldHistoryItems()
        {
            // Arrange
            var note = await _wolkDbContext.CreateAndSaveNote();
            var originalTitle = note.Title;
            var originalContent = note.Content;

            // Act
            for (var i = 0; i < 7; i++)
            {
                note.Content = Guid.NewGuid().ToString();
                note.Title = Guid.NewGuid().ToString();
                await _wolkDbContext.SaveChangesAsync();

                await _handler.Handle(new SaveNoteHistoryNotification {NoteId = note.Id}, CancellationToken.None);
            }

            // Assert
            Assert.AreEqual(_configuration.NoteHistoryLength, note.NoteHistory.Count);
            Assert.IsFalse(note.NoteHistory.Any(h => h.Title == originalTitle || h.Content == originalContent));
        }
    }
}
