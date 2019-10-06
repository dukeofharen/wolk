using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ducode.Wolk.Persistence
{
    public static class PersistenceModule
    {
        public static IServiceCollection AddPersistenceModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IWolkDbContext, WolkDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString(Constants.WolkConnectionStringKey)));
            return services;
        }
    }
}
