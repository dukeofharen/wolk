using Ducode.Wolk.Application.Notes.Models;
using Ducode.Wolk.Domain.Entities.Enums;
using MediatR;

namespace Ducode.Wolk.Application.Notes.Commands.CreateNote
{
    public class CreateNoteCommand : IRequest<NoteDto>
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public NoteType NoteType { get; set; }

        public long NotebookId { get; set; }
    }
}
