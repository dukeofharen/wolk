using Ducode.Wolk.Application.Attachments.Models;
using MediatR;

namespace Ducode.Wolk.Application.Attachments.Queries.GetAttachmentBinary
{
    public class GetAttachmentQuery : IRequest<FullAttachmentDto>
    {
        public long? AttachmentId { get; set; }

        public string Token { get; set; }
    }
}
