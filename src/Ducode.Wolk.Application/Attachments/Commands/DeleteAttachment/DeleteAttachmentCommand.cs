using Ducode.Wolk.Application.Attachments.Models;
using MediatR;

namespace Ducode.Wolk.Application.Attachments.Commands.DeleteAttachment
{
    public class DeleteAttachmentCommand : IRequest
    {
        public long AttachmentId { get; set; }
    }
}
