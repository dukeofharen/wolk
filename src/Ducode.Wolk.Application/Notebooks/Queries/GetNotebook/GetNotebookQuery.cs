using Ducode.Wolk.Application.Notebooks.Models;
using MediatR;

namespace Ducode.Wolk.Application.Notebooks.Queries.GetNotebook
{
    public class GetNotebookQuery : IRequest<NotebookDto>
    {
        public GetNotebookQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
