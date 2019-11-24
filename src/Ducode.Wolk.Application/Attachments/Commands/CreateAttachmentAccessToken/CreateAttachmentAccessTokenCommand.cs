using System;
using Ducode.Wolk.Application.AccessTokens.Models;
using MediatR;

namespace Ducode.Wolk.Application.Attachments.Commands.CreateAttachmentAccessToken
{
    public class CreateAttachmentAccessTokenCommand : IRequest<AccessTokenResultDto>
    {
        public long AttachmentId { get; set; }

        public DateTimeOffset? ExpirationDateTime { get; set; }
    }
}
