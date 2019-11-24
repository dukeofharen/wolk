using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.AccessTokens.Models;
using MediatR;

namespace Ducode.Wolk.Application.Attachments.Commands.CreateAttachmentAccessToken
{
    public class CreateAttachmentAccessTokenCommandHandler : IRequestHandler<CreateAttachmentAccessTokenCommand, AccessTokenResultDto>
    {
        public Task<AccessTokenResultDto> Handle(CreateAttachmentAccessTokenCommand request, CancellationToken cancellationToken) => throw new System.NotImplementedException();
    }
}
