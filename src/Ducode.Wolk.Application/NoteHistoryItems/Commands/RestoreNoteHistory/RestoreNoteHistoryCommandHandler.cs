using System;
using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Application.NoteHistoryItems.Models;
using Ducode.Wolk.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.Application.NoteHistoryItems.Commands.RestoreNoteHistory
{
    public class RestoreNoteHistoryCommandHandler : IRequestHandler<RestoreNoteHistoryCommand>
    {
        private readonly IWolkDbContext _wolkDbContext;

        public RestoreNoteHistoryCommandHandler(IWolkDbContext wolkDbContext)
        {
            _wolkDbContext = wolkDbContext;
        }

        public async Task<Unit> Handle(RestoreNoteHistoryCommand request, CancellationToken cancellationToken)
        {
            var note = await _wolkDbContext.Notes.FirstOrDefaultAsync(n => n.Id == request.NoteId, cancellationToken);
            if (note == null)
            {
                throw new NotFoundException(nameof(Note), request.NoteId);
            }

            var noteHistory =
                await _wolkDbContext.NoteHistory.FirstOrDefaultAsync(h => h.Id == request.NoteHistoryId,
                    cancellationToken);
            if (noteHistory == null)
            {
                throw new NotFoundException(nameof(NoteHistory), request.NoteHistoryId);
            }

            if (noteHistory.NoteId != note.Id)
            {
                throw new ArgumentException(
                    $"Note history with id '{noteHistory.Id}' does not belong to note with ID '{note.Id}'.");
            }

            note.Title = noteHistory.Title;
            note.Content = noteHistory.Content;
            note.NoteType = noteHistory.NoteType;
            await _wolkDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
