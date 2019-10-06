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

            // Add fluent validations
            services.AddValidatorsFromAssemblies(new[] { currentAssembly });

            return services;
        }
    }
}
