using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Interfaces.Authentication;
using Ducode.Wolk.Application.Interfaces.Identity;
using Ducode.Wolk.Common.Constants;
using Ducode.Wolk.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Ducode.Wolk.Api.Middleware
{
    public class TokenRenewalMiddleware
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private readonly RequestDelegate _next;

        public TokenRenewalMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            IOptions<IdentityConfiguration> options,
            ITokenRenewalManager tokenRenewalManager)
        {
            var user = context.User;
            if (user.Identity.IsAuthenticated)
            {
                var expiresAt = ClaimToInt(user, "exp");
                var secondsLeft = (int)(Epoch.AddSeconds(expiresAt) - DateTime.UtcNow).TotalSeconds;
                if (secondsLeft > 0)
                {
                    var percentage = 100 - 100 / (double)options.Value.ExpirationInSeconds * secondsLeft;
                    if (percentage > 80)
                    {
                        // If 80% of the token time is expired, renew the token.
                        var userId = ClaimToInt(user, JwtRegisteredClaimNames.Sub);
                        var jwt = await tokenRenewalManager.RenewTokenAsync(userId);
                        context.Response.Headers.Add(HeaderNames.TokenHeaderKey, jwt);
                    }
                }
            }

            await _next(context);
        }

        private static int ClaimToInt(ClaimsPrincipal user, string key) =>
            int.Parse(user.Claims.Single(c => c.Type == key).Value);
    }
}
