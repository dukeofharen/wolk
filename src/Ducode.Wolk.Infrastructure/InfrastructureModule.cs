using Ducode.Wolk.Common;
using Ducode.Wolk.Infrastructure.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ducode.Wolk.Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructureModule(this IServiceCollection services)
        {
            services.TryAddSingleton<IDateTime, MachineDateTime>();
            services.TryAddSingleton<IFileService, FileService>();
            services.TryAddSingleton<IMimeService, MimeService>();
            return services;
        }
    }
}
