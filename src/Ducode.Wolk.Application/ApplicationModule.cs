using Ducode.Wolk.Application.AccessTokens.Services;
using Ducode.Wolk.Application.Infrastructure.MediatR;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Ducode.Wolk.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection services)
        {
            var currentAssembly = typeof(ApplicationModule).Assembly;

            // Add MediatR
            services.AddMediatR(currentAssembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuditBehavior<,>));

            // Add fluent validations
            services.AddValidatorsFromAssemblies(new[] {currentAssembly});

            // Add services
            services.AddTransient<ICreateAccessTokenService, CreateAccessTokenService>();

            return services;
        }
    }
}
