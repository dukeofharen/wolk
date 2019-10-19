using AutoMapper;
using Ducode.Wolk.Api.Infrastructure.AutoMapper;
using Ducode.Wolk.Application.Infrastructure.AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ducode.Wolk.Api.Tests.Infrastructure.AutoMapper
{
    [TestClass]
    public class AutoMapperConfigurationTest
    {
        [TestMethod]
        public void AutoMapper_CheckConfiguration()
        {
            // Arrange
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationAutoMapperProfile());
                cfg.AddProfile(new ApiAutoMapperProfile());
            });

            // Act / Assert
            config.AssertConfigurationIsValid();
        }
    }
}
