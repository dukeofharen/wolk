using System;

namespace Ducode.Wolk.Application.AccessTokens.Models
{
    public class AccessTokenResultDto
    {
        public string Token { get; set; }

        public DateTimeOffset? ExpirationDateTime { get; set; }
    }
}
