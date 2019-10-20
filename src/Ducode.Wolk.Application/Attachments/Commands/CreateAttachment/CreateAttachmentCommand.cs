using Ducode.Wolk.Application.Attachments.Models;
using MediatR;

namespace Ducode.Wolk.Application.Attachments.Commands.CreateAttachment
{
    public class CreateAttachmentCommand : IRequest<AttachmentDto>
    {
        public string Filename { get; set; }

        public byte[] Contents { get; set; }

        public long NoteId { get; set; }
    }
}
