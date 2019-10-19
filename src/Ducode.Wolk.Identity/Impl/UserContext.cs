using System.Linq;
using System.Security.Claims;
using Ducode.Wolk.Application.Interfaces.Authentication;
using Ducode.Wolk.Common.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Ducode.Wolk.Identity.Impl
{
    internal class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public long CurrentUserId
        {
            get
            {
                var claim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c =>
                    c.Type == JwtRegisteredClaimNames.Sub);
                return claim == null ? 0 : long.Parse(claim.Value);
            }
        }

        public string SecurityStamp
        {
            get
            {
                var claim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c =>
                    c.Type == CustomClaimTypes.SecurityStamp);
                return claim?.Value;
            }
        }
    }
}
