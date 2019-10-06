using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Notebooks.Models;
using Ducode.Wolk.Application.Notebooks.Queries.GetAllNotebooks;
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
    }
}
