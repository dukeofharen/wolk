using System;
using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.AccessTokens.Models;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Domain.Entities;
using Ducode.Wolk.Domain.Entities.Enums;

namespace Ducode.Wolk.Application.AccessTokens.Services
{
    internal class CreateAccessTokenService : ICreateAccessTokenService
    {
        private readonly IWolkDbContext _wolkDbContext;

        public CreateAccessTokenService(IWolkDbContext wolkDbContext)
        {
            _wolkDbContext = wolkDbContext;
        }

        public async Task<AccessTokenResultDto> CreateAccessTokenAsync(
            object identifier,
            AccessTokenType accessTokenType,
            DateTimeOffset? expirationDateTime,
            CancellationToken cancellationToken = default)
        {
            var accessToken = new AccessToken
            {
                Identifier = identifier.ToString(),
                Token = Guid.NewGuid().ToString(),
                AccessTokenType = accessTokenType,
                ExpirationDateTime = expirationDateTime
            };
            _wolkDbContext.AccessTokens.Add(accessToken);
            await _wolkDbContext.SaveChangesAsync(cancellationToken);
            return new AccessTokenResultDto {Token = accessToken.Token, ExpirationDateTime = expirationDateTime};
        }
    }
}
