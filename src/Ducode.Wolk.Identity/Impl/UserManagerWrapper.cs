using System.Threading.Tasks;
using Ducode.Wolk.Domain.Entities;
using Ducode.Wolk.Identity.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Ducode.Wolk.Identity.Impl
{
    internal class UserManagerWrapper : IUserManager
    {
        private readonly UserManager<User> _userManager;

        public UserManagerWrapper(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public Task<IdentityResult> CreateAsync(User user, string password) => _userManager.CreateAsync(user, password);
    }
}
