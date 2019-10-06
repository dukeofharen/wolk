using MediatR;

namespace Ducode.Wolk.Application.Notebooks.Commands.UpdateNotebook
{
    public class UpdateNotebookCommand : IRequest
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
