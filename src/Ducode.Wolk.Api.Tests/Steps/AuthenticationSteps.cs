using System.Threading.Tasks;
using Ducode.Wolk.Application.Interfaces.Identity;
using Ducode.Wolk.Domain.Entities;
using Ducode.Wolk.TestUtilities.FakeData;
using Microsoft.Extensions.DependencyInjection;

namespace Ducode.Wolk.Api.Tests
{
    public partial class IntegrationTestBase
    {
        protected async Task<string> GetJwt(User user = null)
        {
            if (user == null)
            {
                user = await WolkDbContext.CreateAndSaveUser();
            }

            var jwtManager = ServiceProvider.GetService<IJwtManager>();
            return jwtManager.CreateJwt(user);
        }
    }
}
