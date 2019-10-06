using System.Collections.Generic;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Notes.Models;
using Ducode.Wolk.Application.Notes.Queries.GetNotes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ducode.Wolk.Api.Controllers
{
    public class NoteController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetAll() =>
            Ok(await Mediator.Send(new GetNotesQuery()));
    }
}
