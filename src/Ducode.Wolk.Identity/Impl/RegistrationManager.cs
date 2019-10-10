using System.Linq;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Application.Interfaces.Identity;
using Ducode.Wolk.Domain.Entities;
using Ducode.Wolk.Identity.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.Identity.Impl
{
    internal class RegistrationManager : IRegistrationManager
    {
        private readonly IUserManager _userManager;
        private readonly IWolkDbContext _wolkDbContext;

        public RegistrationManager(
            IUserManager userManager,
            IWolkDbContext wolkDbContext)
        {
            _userManager = userManager;
            _wolkDbContext = wolkDbContext;
        }

        public async Task RegisterUserAsync(string email, string password)
        {
            if (await _wolkDbContext.Users.AnyAsync(u => u.NormalizedEmail == email.ToUpper()))
            {
                throw new ConflictException(nameof(User), email);
            }

            var user = new User
            {
                Email = email
            };
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                throw new ValidationException(result.Errors.Select(e => e.Description));
            }
        }
    }
}
