using System.Threading.Tasks;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Application.Interfaces.Identity;
using Ducode.Wolk.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.Identity.Impl
{
    public class TokenRenewalManager : ITokenRenewalManager
    {
        private readonly IJwtManager _jwtManager;
        private readonly IWolkDbContext _wolkDbContext;

        public TokenRenewalManager(IJwtManager jwtManager, IWolkDbContext wolkDbContext)
        {
            _jwtManager = jwtManager;
            _wolkDbContext = wolkDbContext;
        }

        public async Task<string> RenewTokenAsync(long userId)
        {
            var user = await _wolkDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new NotFoundException(nameof(User), userId);
            }

            return _jwtManager.CreateJwt(user);
        }
    }
}
