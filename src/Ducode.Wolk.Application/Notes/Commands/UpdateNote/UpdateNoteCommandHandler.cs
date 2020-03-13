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

namespace Ducode.Wolk.Application.Notes.Commands.UpdateNote
{
    public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand>
    {
        private readonly ILogger<UpdateNoteCommandHandler> _logger;
        private readonly WolkConfiguration _configuration;
        private readonly IWolkDbContext _wolkDbContext;

        public UpdateNoteCommandHandler(
            ILogger<UpdateNoteCommandHandler> logger,
            IOptions<WolkConfiguration> options,
            IWolkDbContext wolkDbContext)
        {
            _logger = logger;
            _configuration = options.Value;
            _wolkDbContext = wolkDbContext;
        }

        public async Task<Unit> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
        {
            var note =
                await _wolkDbContext.Notes
                    .FirstOrDefaultAsync(n => n.Id == request.Id, cancellationToken);
            if (note == null)
            {
                throw new NotFoundException(nameof(Note), request.Id);
            }

            // Remove older history items.
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

            note.Title = request.Title;
            note.Content = request.Content;
            note.NoteType = request.NoteType;
            note.NotebookId = request.NotebookId;
            await _wolkDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
