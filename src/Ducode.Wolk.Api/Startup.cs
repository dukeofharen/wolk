using System.Linq;
using System.Reflection;
using AutoMapper;
using Ducode.Wolk.Api.Attributes;
using Ducode.Wolk.Api.Middleware;
using Ducode.Wolk.Api.Utilities;
using Ducode.Wolk.Application;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Application.Interfaces.Identity;
using Ducode.Wolk.Configuration;
using Ducode.Wolk.Identity;
using Ducode.Wolk.Infrastructure;
using Ducode.Wolk.Persistence;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSwag;
using NSwag.Generation.Processors.Security;

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
            services
                .AddHttpContextAccessor()
                .AddApplicationModule()
                .AddInfrastructureModule()
                .AddPersistenceModule(Configuration)
                .AddIdentityModule(Configuration)
                .AddAutoMapper(
                    config => config.AllowNullCollections = true,
                    typeof(Startup).Assembly,
                    typeof(ApplicationModule).Assembly)
                .AddOpenApiDocument(c =>
                {
                    c.Title = "Wolk API";
                    c.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
                    c.DocumentProcessors.Add(new SecurityDefinitionAppender("JWT",
                        new OpenApiSecurityScheme
                        {
                            Type = OpenApiSecuritySchemeType.ApiKey,
                            Name = "Authorization",
                            Description = "Copy 'Bearer ' + valid JWT token into field",
                            In = OpenApiSecurityApiKeyLocation.Header
                        }));
                })
                .AddValidatorsFromAssemblies(new[] {typeof(Startup).Assembly});

            services
                .AddOptions<WolkConfiguration>()
                .Configure<IConfiguration>((opt, conf) => Configuration.Bind(nameof(WolkConfiguration), opt));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) =>
            ConfigureInternal(app, env, true, true);

        internal void ConfigureInternal(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            bool executeMigration,
            bool loadStaticFiles)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseMiddleware<ValidUserMiddleware>();
            app.UseMiddleware<TokenRenewalMiddleware>();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseGui(loadStaticFiles);

            // Ensure the database is created and a "default" user is created (if configured).
            if (executeMigration)
            {
                using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();
                var context = (WolkDbContext)scope.ServiceProvider.GetRequiredService<IWolkDbContext>();
                logger.LogInformation("Attempting to run database migrations.");
                context.Database.Migrate();
                logger.LogInformation("Database migrations executed successfully.");

                var defaultUserCreator = scope.ServiceProvider.GetRequiredService<IDefaultUserCreator>();
                defaultUserCreator.CreateOrUpdateDefaultUser().Wait();
            }
        }
    }
}
