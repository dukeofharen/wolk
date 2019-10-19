using System.Net;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Interfaces.Authentication;
using Ducode.Wolk.Application.Users.Queries.UserValid;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Ducode.Wolk.Api.Middleware
{
    public class ValidUserMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            IMediator mediator,
            IUserContext userContext)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var request = new UserValidQuery
                {
                    UserId = userContext.CurrentUserId, SecurityStamp = userContext.SecurityStamp
                };
                if (!await mediator.Send(request))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
                }
            }

            await _next(context);
        }
    }
}
