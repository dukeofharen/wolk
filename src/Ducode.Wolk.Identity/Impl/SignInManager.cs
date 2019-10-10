using System.Threading.Tasks;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Application.Interfaces.Identity;
using Ducode.Wolk.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.Identity.Impl
{
    internal class SignInManager : ISignInManager
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IWolkDbContext _context;

        public SignInManager(IPasswordHasher<User> passwordHasher, IWolkDbContext context)
        {
            _passwordHasher = passwordHasher;
            _context = context;
        }

        public async Task<bool> CheckCredentialsAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                throw new UnauthorizedException();
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success ||
                   result == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}
