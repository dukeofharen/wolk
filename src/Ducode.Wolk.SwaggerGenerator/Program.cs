using System;
using System.IO;
using System.Threading.Tasks;
using Ducode.Wolk.Api;
using Ducode.Wolk.Common.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Ducode.Wolk.SwaggerGenerator
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            // Mock settings.
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
                .Build();

            // This program hosts Wolk in memory, retrieves the contents of the swagger.json file and saves it.
            var mockEnv = new Mock<IWebHostEnvironment>();
            mockEnv.Setup(m => m.EnvironmentName).Returns("Development");

            var testServer = new TestServer(
                new WebHostBuilder()
                    .ConfigureServices(services => Startup.ConfigureServicesStatic(services, config))
                    .Configure(appBuilder => Startup.ConfigureStatic(appBuilder, mockEnv.Object, false, false)));
            var client = testServer.CreateClient();

            // Retrieve the Swagger URL.
            using var response = await client.GetAsync("swagger/v1/swagger.json");
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"The call to the swagger.json URL failed with an HTTP '{response.StatusCode}'.");
            }

            var content = await response.Content.ReadAsStringAsync();
            var pathToSave = Path.Join(AssemblyHelper.GetCallingAssemblyRootPath(), "swagger.json");
            File.WriteAllText(pathToSave, content);
        }
    }
}
