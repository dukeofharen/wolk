using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Domain.Entities;
using Ducode.Wolk.Identity.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Ducode.Wolk.Identity.Impl
{
    internal class UserManagerWrapper : IUserManager
    {
        private readonly UserManager<User> _userManager;
        private readonly IWolkDbContext _wolkDbContext;

        public UserManagerWrapper(
            UserManager<User> userManager,
            IWolkDbContext wolkDbContext)
        {
            _userManager = userManager;
            _wolkDbContext = wolkDbContext;
        }

        public Task<IdentityResult> CreateAsync(User user, string password) => _userManager.CreateAsync(user, password);

        public async Task UpdatePasswordAsync(User user, string password)
        {
            await AsserCorrectIdentityResult(() => _userManager.RemovePasswordAsync(user));
            await AsserCorrectIdentityResult(() => _userManager.AddPasswordAsync(user, password));
            await _wolkDbContext.SaveChangesAsync(CancellationToken.None);
        }

        private async Task AsserCorrectIdentityResult(Func<Task<IdentityResult>> action)
        {
            var result = await action();
            if (!result.Succeeded)
            {
                throw new ValidationException(result.Errors.Select(e => e.Description));
            }
        }
    }
}
