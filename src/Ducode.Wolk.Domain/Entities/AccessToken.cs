using System;
using Ducode.Wolk.Domain.Entities.Enums;

namespace Ducode.Wolk.Domain.Entities
{
    /// <summary>
    /// An entity which is used for storing access token, which are used to access entities without the need for authentication.
    /// </summary>
    public class AccessToken : BaseEntity
    {
        public string Token { get; set; }

        public DateTimeOffset? ExpirationDateTime { get; set; }

        public AccessTokenType AccessTokenType { get; set; }

        public string Identifier { get; set; }
    }
}
