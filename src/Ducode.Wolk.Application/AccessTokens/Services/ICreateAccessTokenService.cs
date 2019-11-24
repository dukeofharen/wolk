using System;
using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.AccessTokens.Models;
using Ducode.Wolk.Domain.Entities.Enums;

namespace Ducode.Wolk.Application.AccessTokens.Services
{
    public interface ICreateAccessTokenService
    {
        Task<AccessTokenResultDto> CreateAccessTokenAsync(
            object identifier,
            AccessTokenType accessTokenType,
            DateTimeOffset? expirationDateTime,
            CancellationToken cancellationToken = default);
    }
}
