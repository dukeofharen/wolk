using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Domain;
using Ducode.Wolk.Persistence.SaveChanges;
using Ducode.Wolk.Persistence.SaveChanges.Impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ducode.Wolk.Persistence
{
    public static class PersistenceModule
    {
        public static IServiceCollection AddPersistenceModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IWolkDbContext, WolkDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString(Constants.WolkConnectionStringKey)));

            // Save changes handlers
            services.TryAddTransient<ISaveChangesHandler, ChangeDateSaveChangesHandler>();

            return services;
        }
    }
}
