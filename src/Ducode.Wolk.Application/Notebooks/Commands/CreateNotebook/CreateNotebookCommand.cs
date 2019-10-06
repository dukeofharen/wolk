using Ducode.Wolk.Application.Notebooks.Models;
using MediatR;

namespace Ducode.Wolk.Application.Notebooks.Commands.CreateNotebook
{
    public class CreateNotebookCommand : IRequest<NotebookDto>
    {
        public string Name { get; set; }
    }
}
