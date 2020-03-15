using System;
using System.Net;
using Ducode.Wolk.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Ducode.Wolk.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<CustomExceptionFilterAttribute> _logger;

        public CustomExceptionFilterAttribute(ILogger<CustomExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException validationException)
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new JsonResult(
                    validationException.Failures);
                return;
            }

            var exception = context.Exception;
            _logger.LogInformation($"Exception of type '{exception.GetType().Name}' thrown.");

            var message = exception.Message;
            var code = HttpStatusCode.InternalServerError;
            switch (exception)
            {
                case NotFoundException _:
                    code = HttpStatusCode.NotFound;
                    break;
                case ConflictException _:
                    code = HttpStatusCode.Conflict;
                    break;
                case UnauthorizedException _:
                    code = HttpStatusCode.Unauthorized;
                    break;
                case ArgumentException _:
                    code = HttpStatusCode.BadRequest;
                    break;
                default:
                    message = "Unhandled exception thrown!";
                    _logger.LogError(exception, message);
                    break;
            }

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)code;
            context.Result = new JsonResult(new {error = new[] {message}});
        }
    }
}
