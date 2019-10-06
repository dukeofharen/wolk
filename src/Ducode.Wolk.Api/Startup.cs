using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Ducode.Wolk.Application;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Infrastructure;
using Ducode.Wolk.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ducode.Wolk.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddApplicationModule();
            services.AddInfrastructureModule();
            services.AddPersistenceModule(Configuration);
            services.AddAutoMapper(
                config => config.AllowNullCollections = true,
                typeof(Startup).Assembly,
                typeof(ApplicationModule).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            // Ensure the database is created.
            using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var context = (WolkDbContext)scope.ServiceProvider.GetRequiredService<IWolkDbContext>();
            context.Database.Migrate();
        }
    }
}
