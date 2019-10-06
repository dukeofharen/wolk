using MediatR;

namespace Ducode.Wolk.Application.Notebooks.Commands.DeleteNotebook
{
    public class DeleteNotebookCommand : IRequest
    {
        public DeleteNotebookCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
