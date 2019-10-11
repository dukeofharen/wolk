using System.Reflection;
using AutoMapper;
using Ducode.Wolk.Api.Attributes;
using Ducode.Wolk.Application;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Identity;
using Ducode.Wolk.Infrastructure;
using Ducode.Wolk.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ducode.Wolk.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(o => o.Filters.Add(typeof(CustomExceptionFilterAttribute)))
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddApplicationPart(Assembly.GetExecutingAssembly());
//            services.AddControllers();
            services
                .AddApplicationModule()
                .AddInfrastructureModule()
                .AddPersistenceModule(Configuration)
                .AddIdentityModule(Configuration)
                .AddAutoMapper(
                    config => config.AllowNullCollections = true,
                    typeof(Startup).Assembly,
                    typeof(ApplicationModule).Assembly);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) => ConfigureInternal(app, env, true);

        internal void ConfigureInternal(IApplicationBuilder app, IWebHostEnvironment env, bool executeMigration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            // Ensure the database is created.
            if (executeMigration)
            {
                using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
                var context = (WolkDbContext)scope.ServiceProvider.GetRequiredService<IWolkDbContext>();
                context.Database.Migrate();
            }
        }
    }
}
