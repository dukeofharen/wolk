using System;
using System.Net;
using Ducode.Wolk.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ducode.Wolk.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException exception)
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new JsonResult(
                    exception.Failures);
                return;
            }

            var code = HttpStatusCode.InternalServerError;
            if (context.Exception is NotFoundException)
            {
                code = HttpStatusCode.NotFound;
            }
            else if (context.Exception is ConflictException)
            {
                code = HttpStatusCode.Conflict;
            }
            else if (context.Exception is UnauthorizedException)
            {
                code = HttpStatusCode.Unauthorized;
            }

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)code;
            context.Result = new JsonResult(new {error = new[] {context.Exception.Message}});
        }
    }
}
