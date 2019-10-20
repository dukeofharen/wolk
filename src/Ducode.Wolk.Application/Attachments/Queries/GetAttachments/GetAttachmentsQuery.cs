using System.Collections.Generic;
using Ducode.Wolk.Application.Attachments.Models;
using MediatR;

namespace Ducode.Wolk.Application.Attachments.Queries.GetAttachments
{
    public class GetAttachmentsQuery : IRequest<IEnumerable<AttachmentDto>>
    {
        public long NoteId { get; set; }
    }
}
