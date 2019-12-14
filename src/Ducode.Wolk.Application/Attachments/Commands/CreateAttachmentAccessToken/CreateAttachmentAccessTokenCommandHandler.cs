using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.AccessTokens.Models;
using Ducode.Wolk.Application.AccessTokens.Services;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Domain.Entities;
using Ducode.Wolk.Domain.Entities.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.Application.Attachments.Commands.CreateAttachmentAccessToken
{
    public class CreateAttachmentAccessTokenCommandHandler : IRequestHandler<
        CreateAttachmentAccessTokenCommand,
        AccessTokenResultDto>
    {
        private readonly ICreateAccessTokenService _createAccessTokenService;
        private readonly IWolkDbContext _wolkDbContext;

        public CreateAttachmentAccessTokenCommandHandler(
            ICreateAccessTokenService createAccessTokenService,
            IWolkDbContext wolkDbContext)
        {
            _createAccessTokenService = createAccessTokenService;
            _wolkDbContext = wolkDbContext;
        }

        public async Task<AccessTokenResultDto> Handle(
            CreateAttachmentAccessTokenCommand request,
            CancellationToken cancellationToken)
        {
            var attachment = await _wolkDbContext.Attachments
                .FirstOrDefaultAsync(a => a.Id == request.AttachmentId, cancellationToken);
            if (attachment == null)
            {
                throw new NotFoundException(nameof(Attachment), request.AttachmentId);
            }

            return await _createAccessTokenService.CreateAccessTokenAsync(
                attachment.Id,
                AccessTokenType.Attachment,
                request.ExpirationDateTime,
                cancellationToken);
        }
    }
}
