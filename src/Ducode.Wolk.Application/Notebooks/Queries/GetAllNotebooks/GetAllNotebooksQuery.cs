using System.Collections;
using System.Collections.Generic;
using Ducode.Wolk.Application.Notebooks.Models;
using MediatR;

namespace Ducode.Wolk.Application.Notebooks.Queries.GetAllNotebooks
{
    public class GetAllNotebooksQuery : IRequest<IEnumerable<NotebookDto>>
    {
    }
}
