using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Configuration;
using Ducode.Wolk.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ducode.Wolk.Application.NoteHistoryItems.Notifications.SaveNoteHistory
{
    public class SaveNoteHistoryNotificationHandler : INotificationHandler<SaveNoteHistoryNotification>
    {
        private readonly ILogger<SaveNoteHistoryNotificationHandler> _logger;
        private readonly IWolkDbContext _wolkDbContext;
        private readonly WolkConfiguration _configuration;

        public SaveNoteHistoryNotificationHandler(
            ILogger<SaveNoteHistoryNotificationHandler> logger,
            IWolkDbContext wolkDbContext,
            IOptions<WolkConfiguration> options)
        {
            _logger = logger;
            _wolkDbContext = wolkDbContext;
            _configuration = options.Value;
        }

        public async Task Handle(SaveNoteHistoryNotification notification, CancellationToken cancellationToken)
        {
            var note =
                await _wolkDbContext.Notes
                    .FirstOrDefaultAsync(n => n.Id == notification.NoteId, cancellationToken);
            if (note == null)
            {
                throw new NotFoundException(nameof(Note), notification.NoteId);
            }

            var oldHistory = await _wolkDbContext.NoteHistory
                .Where(h => h.NoteId == note.Id)
                .OrderByDescending(h => h.Id)
                .Skip(_configuration.NoteHistoryLength - 1) // -1 is needed because a new item will be added after this.
                .ToArrayAsync(cancellationToken);
            if (oldHistory.Any())
            {
                _logger.LogInformation($"Deleting {oldHistory.Length} old note history items.");
                _wolkDbContext.NoteHistory.RemoveRange(oldHistory);
            }

            var historyNote = new NoteHistory
            {
                Content = note.Content,
                NoteId = note.Id,
                Title = note.Title,
                NoteType = note.NoteType,
                OriginalChanged = note.Changed,
                OriginalCreated = note.Created
            };
            _wolkDbContext.NoteHistory.Add(historyNote);
            await _wolkDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
