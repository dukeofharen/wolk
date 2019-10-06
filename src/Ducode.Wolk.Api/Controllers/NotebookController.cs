using System.Collections.Generic;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Notebooks.Models;
using Ducode.Wolk.Application.Notebooks.Queries.GetAllNotebooks;
using Ducode.Wolk.Application.Notebooks.Queries.GetNotebook;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ducode.Wolk.Api.Controllers
{
    public class NotebookController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<NotebookDto>>> GetAll() =>
            Ok(await Mediator.Send(new GetAllNotebooksQuery()));

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<NotebookDto>> Get(long id) =>
            Ok(await Mediator.Send(new GetNotebookQuery(id)));
    }
}
