using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Persistence;
using Ducode.Wolk.TestUtilities.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Ducode.Wolk.Api.Tests
{
    public abstract partial class IntegrationTestBase
    {
        protected TestServer TestServer;

        protected HttpClient HttpClient;

        protected WolkDbContext WolkDbContext => (WolkDbContext)ServiceProvider.GetService<IWolkDbContext>();

        protected string BaseAddress => TestServer.BaseAddress.ToString();

        protected IServiceProvider ServiceProvider;

        [TestInitialize]
        public void InitializeIntegrationTest()
        {
            var servicesToReplace = new List<(Type, object)>();

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"IdentityConfiguration:JwtSecret", "2346sedrfgsrahyjrtyserASD@"},
                    {"IdentityConfiguration:ExpirationInSeconds", "10"}
                })
                .Build();
            var startup = new Startup(config);

            var wolkDbContext = InMemoryDbContextFactory.Create();
            servicesToReplace.Add((typeof(IWolkDbContext), wolkDbContext));

            var mockEnvironment = new Mock<IWebHostEnvironment>();
            mockEnvironment
                .Setup(m => m.EnvironmentName)
                .Returns("Production");

            TestServer = new TestServer(new WebHostBuilder()
                .ConfigureServices(services => ConfigureServices(startup, services, servicesToReplace))
                .Configure(app => startup.ConfigureInternal(app, mockEnvironment.Object, false)));
            HttpClient = TestServer.CreateClient();
        }

        [TestCleanup]
        public void CleanupIntegrationTest()
        {
            WolkDbContext.Destroy();
            TestServer.Dispose();
        }

        private void ConfigureServices(
            Startup startup,
            IServiceCollection services,
            IList<(Type, object)> servicesToReplace)
        {
            // Delete old services
            var servicesToDelete = servicesToReplace
                .Select(str => str.Item1)
                .ToArray();
            var serviceDescriptors = services
                .Where(s => servicesToDelete.Contains(s.ServiceType))
                .ToArray();
            foreach (var descriptor in serviceDescriptors)
            {
                services.Remove(descriptor);
            }

            // Add mock services
            foreach (var service in servicesToReplace)
            {
                services.AddTransient(service.Item1, serviceProvider => service.Item2);
            }

            startup.ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
