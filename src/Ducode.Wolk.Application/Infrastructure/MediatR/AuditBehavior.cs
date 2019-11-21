using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.Infrastructure.MediatR.Attributes;
using Ducode.Wolk.Application.Interfaces.Authentication;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ducode.Wolk.Application.Infrastructure.MediatR
{
    public class AuditBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<AuditBehavior<TRequest, TResponse>> _logger;
        private readonly IUserContext _userContext;

        public AuditBehavior(
            ILogger<AuditBehavior<TRequest, TResponse>> logger,
            IUserContext userContext = null)
        {
            _logger = logger;
            _userContext = userContext;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var noAuditing = request.GetType().IsDefined(typeof(NoAuditAttribute), true);
            if (noAuditing)
            {
                return await next();
            }

            var builder = new StringBuilder();
            builder.AppendLine("Audit:");

            var type = request.GetType();
            builder.AppendLine($"Handling request '{type.FullName}'");
            builder.AppendLine($"Input: {SanitizedJson(request)}");

            if (_userContext?.CurrentUserId > 0)
            {
                builder.AppendLine($"User ID: {_userContext.CurrentUserId}");
            }

            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var result = await next();
                stopwatch.Stop();

                builder.AppendLine($"Duration: {stopwatch.Elapsed.Milliseconds} ms");
                return result;
            }
            catch (ValidationException ex)
            {
                builder.AppendLine($"{nameof(ValidationException)} thrown");
                foreach (var error in ex.Failures)
                {
                    builder.AppendLine($"- {error}");
                }

                throw;
            }
            catch (Exception ex)
            {
                builder.AppendLine($"{ex.GetType()} thrown: {ex}");
                throw;
            }
            finally
            {
                _logger.LogDebug(builder.ToString());
            }
        }

        private string SanitizedJson(object input)
        {
            var forbiddenKeywords = new[] {"content", "contents", "password"};
            var json = JsonConvert.SerializeObject(input);
            var jobject = JObject.Parse(json);
            var result = new Dictionary<string, string>();
            foreach (var element in jobject)
            {
                var value = element.Value.ToString();
                if (forbiddenKeywords.Any(w => element.Key.IndexOf(w, StringComparison.OrdinalIgnoreCase) > -1))
                {
                    value = "*****";
                }

                result.Add(element.Key, value);
            }

            return JsonConvert.SerializeObject(result);
        }
    }
}
