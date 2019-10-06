using MediatR;

namespace Ducode.Wolk.Application.Notes.Commands.DeleteNote
{
    public class DeleteNoteCommand : IRequest
    {
        public DeleteNoteCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
