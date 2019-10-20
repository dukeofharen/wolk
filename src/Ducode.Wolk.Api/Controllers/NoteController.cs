using System.Collections.Generic;
using System.Threading.Tasks;
using Ducode.Wolk.Api.Models.Notes;
using Ducode.Wolk.Application.Attachments.Models;
using Ducode.Wolk.Application.Attachments.Queries.GetAttachments;
using Ducode.Wolk.Application.Notes.Commands.CreateNote;
using Ducode.Wolk.Application.Notes.Commands.DeleteNote;
using Ducode.Wolk.Application.Notes.Commands.UpdateNote;
using Ducode.Wolk.Application.Notes.Models;
using Ducode.Wolk.Application.Notes.Queries.GetNote;
using Ducode.Wolk.Application.Notes.Queries.GetNotes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ducode.Wolk.Api.Controllers
{
    public class NoteController : BaseApiController
    {
        /// <summary>
        /// Returns a list of all notes. It only returns the preview per note, not the complete note contents.
        /// </summary>
        /// <param name="query">The query parameters.</param>
        /// <returns>A list of notes.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<NoteOverviewDto>>> GetAll([FromQuery]GetNotesQuery query) =>
            Ok(Mapper.Map<IEnumerable<NoteOverviewDto>>(await Mediator.Send(query)));

        /// <summary>
        /// Returns a specific note. This returns the whole note, including the complete contents.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<NoteDto>> Get([FromRoute]long id) =>
            Ok(await Mediator.Send(new GetNoteQuery(id)));

        /// <summary>
        /// Creates a new note.
        /// </summary>
        /// <param name="noteModel">The note.</param>
        /// <returns>The added note.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<NoteDto>> Create([FromBody] MutateNoteModel noteModel)
        {
            var note = await Mediator.Send(Mapper.Map<CreateNoteCommand>(noteModel));
            return CreatedAtAction(nameof(Get), new { id = note.Id}, note);
        }

        /// <summary>
        /// Updates an existing note.
        /// </summary>
        /// <param name="noteModel">The note.</param>
        /// <param name="id">The note ID.</param>
        /// <returns>No content.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update([FromBody] MutateNoteModel noteModel, [FromRoute]long id)
        {
            var command = Mapper.Map<UpdateNoteCommand>(noteModel);
            command.Id = id;
            await Mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Deletes a specific note.
        /// </summary>
        /// <param name="id">The note ID.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete([FromRoute] long id)
        {
            await Mediator.Send(new DeleteNoteCommand(id));
            return NoContent();
        }

        /// <summary>
        /// Returns a list of all attachments for a specific note. This does not include the actual attachment contents, just the metadata.
        /// </summary>
        /// <param name="query">The query parameters.</param>
        /// <returns>A list of attachment metadata.</returns>
        [HttpGet("{noteId}/attachments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AttachmentDto>>> GetAllAttachments(
            [FromRoute] GetAttachmentsQuery query) =>
            Ok(Mapper.Map<IEnumerable<AttachmentDto>>(await Mediator.Send(query)));
    }
}
