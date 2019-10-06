using MediatR;

namespace Ducode.Wolk.Application.Notes.Commands.UpdateNote
{
    public class UpdateNoteCommand : IRequest
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public long NotebookId { get; set; }
    }
}
