using System;
using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Domain.Entities;
using Ducode.Wolk.Domain.Entities.Enums;

namespace Ducode.Wolk.TestUtilities.FakeData
{
    public static class AccessTokenFakeDataGenerator
    {
        public static async Task<AccessToken> CreateAndSaveAttachmentAccessToken(
            this IWolkDbContext wolkDbContext,
            Attachment attachment,
            DateTimeOffset? expirationDateTime = null) =>
            await wolkDbContext.CreateAndSaveAttachmentAccessToken(attachment.Id, expirationDateTime);

        public static async Task<AccessToken> CreateAndSaveAttachmentAccessToken(
            this IWolkDbContext wolkDbContext,
            object identifier,
            DateTimeOffset? expirationDateTime = null)
        {
            var accessToken = new AccessToken
            {
                Identifier = identifier.ToString(),
                Token = Guid.NewGuid().ToString(),
                AccessTokenType = AccessTokenType.Attachment,
                ExpirationDateTime = expirationDateTime
            };
            wolkDbContext.AccessTokens.Add(accessToken);
            await wolkDbContext.SaveChangesAsync(CancellationToken.None);
            return accessToken;
        }
    }
}
