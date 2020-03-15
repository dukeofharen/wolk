using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Application.NoteHistoryItems.Notifications.SaveNoteHistory;
using Ducode.Wolk.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.Application.Notes.Commands.UpdateNote
{
    public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand>
    {
        private readonly IMediator _mediator;
        private readonly IWolkDbContext _wolkDbContext;

        public UpdateNoteCommandHandler(
            IMediator mediator,
            IWolkDbContext wolkDbContext)
        {
            _mediator = mediator;
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

            await _mediator.Publish(new SaveNoteHistoryNotification {NoteId = note.Id}, cancellationToken);

            note.Title = request.Title;
            note.Content = request.Content;
            note.NoteType = request.NoteType;
            note.NotebookId = request.NotebookId;
            await _wolkDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
