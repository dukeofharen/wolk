using MediatR;

namespace Ducode.Wolk.Application.NoteHistoryItems.Commands.RestoreNoteHistory
{
    public class RestoreNoteHistoryCommand : IRequest
    {
        public long NoteId { get; set; }

        public long NoteHistoryId { get; set; }
    }
}
