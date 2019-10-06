using System.Collections.Generic;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Notebooks.Commands.CreateNotebook;
using Ducode.Wolk.Application.Notebooks.Commands.UpdateNotebook;
using Ducode.Wolk.Application.Notebooks.Models;
using Ducode.Wolk.Application.Notebooks.Queries.GetAllNotebooks;
using Ducode.Wolk.Application.Notebooks.Queries.GetNotebook;
using Ducode.Wolk.Application.Notes.Models;
using Ducode.Wolk.Application.Notes.Queries.GetNotes;
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

        [HttpGet("{id}/notes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<NoteDto>> GetNotesInNotebook(long id) =>
            Ok(await Mediator.Send(new GetNotesQuery {NotebookId = id}));

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<NotebookDto>> Create([FromBody] CreateNotebookCommand command)
        {
            var notebook = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id = notebook.Id}, notebook);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Update([FromBody] UpdateNotebookCommand command, [FromRoute]long id)
        {
            command.Id = id;
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
