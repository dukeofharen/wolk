using System.Net;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Interfaces.Authentication;
using Ducode.Wolk.Application.Users.Queries.UserValid;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

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
            ILogger<ValidUserMiddleware> logger,
            IMediator mediator,
            IUserContext userContext)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var userId = userContext.CurrentUserId;
                var request = new UserValidQuery {UserId = userId, SecurityStamp = userContext.SecurityStamp};
                if (!await mediator.Send(request))
                {
                    logger.LogInformation(
                        $"User with ID '{userId}' not valid anymore because security stamp has changed.");
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
                }
            }

            await _next(context);
        }
    }
}
