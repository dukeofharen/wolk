using Ducode.Wolk.Application.Notes.Models;
using MediatR;

namespace Ducode.Wolk.Application.Notes.Commands.CreateNote
{
    public class CreateNoteCommand : IRequest<NoteDto>
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public long NotebookId { get; set; }
    }
}
