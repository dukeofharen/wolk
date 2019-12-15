using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Domain.Entities;
using Ducode.Wolk.Identity.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.Identity.Impl
{
    internal class UserManagerWrapper : IUserManager
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly UserManager<User> _userManager;
        private readonly IWolkDbContext _wolkDbContext;

        public UserManagerWrapper(
            IPasswordHasher<User> passwordHasher,
            UserManager<User> userManager,
            IWolkDbContext wolkDbContext)
        {
            _passwordHasher = passwordHasher;
            _userManager = userManager;
            _wolkDbContext = wolkDbContext;
        }

        public Task<IdentityResult> CreateAsync(User user, string password) => _userManager.CreateAsync(user, password);

        public async Task UpdatePasswordAsync(User user, string password)
        {
            var actualUser = await _wolkDbContext.Users.SingleAsync(u => u.Id == user.Id);
            actualUser.PasswordHash = _passwordHasher.HashPassword(actualUser, password);
            await _wolkDbContext.SaveChangesAsync(CancellationToken.None);
        }
    }
}
