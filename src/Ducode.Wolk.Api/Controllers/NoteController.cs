using System.Collections.Generic;
using System.Threading.Tasks;
using Ducode.Wolk.Api.Models.Attachments;
using Ducode.Wolk.Api.Models.Notes;
using Ducode.Wolk.Application.AccessTokens.Models;
using Ducode.Wolk.Application.Attachments.Commands.CreateAttachment;
using Ducode.Wolk.Application.Attachments.Commands.CreateAttachmentAccessToken;
using Ducode.Wolk.Application.Attachments.Commands.DeleteAttachment;
using Ducode.Wolk.Application.Attachments.Models;
using Ducode.Wolk.Application.Attachments.Queries.GetAttachmentBinary;
using Ducode.Wolk.Application.Attachments.Queries.GetAttachments;
using Ducode.Wolk.Application.NoteHistoryItems.Models;
using Ducode.Wolk.Application.NoteHistoryItems.Queries.GetNoteHistory;
using Ducode.Wolk.Application.Notes.Commands.CreateNote;
using Ducode.Wolk.Application.Notes.Commands.DeleteNote;
using Ducode.Wolk.Application.Notes.Commands.UpdateNote;
using Ducode.Wolk.Application.Notes.Models;
using Ducode.Wolk.Application.Notes.Queries.GetNote;
using Ducode.Wolk.Application.Notes.Queries.GetNotes;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult<IEnumerable<NoteDto>>> GetAll([FromQuery] GetNotesQuery query) =>
            Ok(Mapper.Map<IEnumerable<NoteDto>>(await Mediator.Send(query)));

        /// <summary>
        /// Returns a specific note. This returns the whole note, including the complete contents.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<NoteDto>> Get([FromRoute] long id) =>
            Ok(await Mediator.Send(new GetNoteQuery(id) {UpdateOpenedDateTime = true}));

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
            return CreatedAtAction(nameof(Get), new {id = note.Id}, note);
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
        public async Task<ActionResult> Update([FromBody] MutateNoteModel noteModel, [FromRoute] long id)
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

        /// <summary>
        /// Adds a new file for a given note.
        /// </summary>
        /// <param name="noteId">The note ID.</param>
        /// <param name="model">The uploaded file.</param>
        /// <returns>The added file.</returns>
        [HttpPost("{noteId}/attachments")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<AttachmentDto>> CreateAttachment(
            [FromRoute] long noteId,
            [FromBody] MutateAttachmentModel model)
        {
            var command = Mapper.Map<CreateAttachmentCommand>(model);
            command.NoteId = noteId;
            var result = await Mediator.Send(command);
            return CreatedAtAction(
                nameof(GetAttachment),
                new {noteId, attachmentId = result.Id},
                result);
        }

        /// <summary>
        /// Returns the actual attachment as binary.
        /// </summary>
        /// <param name="attachmentId">The attachment ID.</param>
        /// <returns>The attachment as binary.</returns>
        [HttpGet("{noteId}/attachments/{attachmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAttachment([FromRoute] long attachmentId)
        {
            var query = new GetAttachmentQuery {AttachmentId = attachmentId};
            var attachment = await Mediator.Send(query);
            return File(attachment.Contents, attachment.MimeType, attachment.Filename);
        }

        /// <summary>
        /// Retrieves all past mutations of a specific note.
        /// </summary>
        /// <param name="noteId">The ID of the note.</param>
        /// <param name="includeFullContent">Whether the full content should be returned, or only a preview.</param>
        /// <returns></returns>
        [HttpGet("{noteId}/history")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<NoteHistoryDto>>> GetNoteHistory(
            [FromRoute] long noteId,
            [FromQuery] bool includeFullContent) =>
            Ok(Mapper.Map<IEnumerable<NoteHistoryDto>>(
                await Mediator.Send(new GetNoteHistoryQuery
                {
                    NoteId = noteId, IncludeFullContent = includeFullContent
                })));

        [HttpPut("{noteId}/history/{noteHistoryId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RestoreNoteHistory(
            [FromRoute] long noteId,
            [FromRoute] long noteHistoryId)
        {
            return NoContent();
        }

        /// <summary>
        /// Deletes a given attachment.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{noteId}/attachments/{attachmentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteAttachment([FromRoute] DeleteAttachmentCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// An endpoint where access tokens can be created for sharing attachments.
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="attachmentId">The attachment ID.</param>
        /// <param name="model">The model.</param>
        /// <returns>The created token.</returns>
        [HttpPost("{noteId}/attachments/{attachmentId}/accessTokens")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AccessTokenResultDto>> CreateAttachmentAccessToken(
            [FromRoute] long noteId,
            [FromRoute] long attachmentId,
            [FromBody] MutateAttachmentAccessTokenModel model)
        {
            var command = Mapper.Map<CreateAttachmentAccessTokenCommand>(model);
            command.AttachmentId = attachmentId;
            var result = await Mediator.Send(command);
            return CreatedAtAction(
                nameof(GetAttachmentByAccessToken),
                new {noteId, attachmentId, token = result.Token, filename = result.Filename},
                result);
        }

        /// <summary>
        /// An endpoint for retrieving attachment contents by access token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns>The attachment binary.</returns>
        [AllowAnonymous]
        [HttpGet("{noteId}/attachments/{attachmentId}/accessTokens/{token}/{filename?}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAttachmentByAccessToken([FromRoute] string token)
        {
            var query = new GetAttachmentQuery {Token = token};
            var attachment = await Mediator.Send(query);
            return File(attachment.Contents, attachment.MimeType, attachment.Filename);
        }
    }
}
