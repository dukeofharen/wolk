using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Ducode.Wolk.Identity
{
    public class UserStore :
        IUserPasswordStore<User>,
        IUserEmailStore<User>
    {
        private readonly IWolkDbContext _context;

        public UserStore(IWolkDbContext context)
        {
            _context = context;
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
        }

        public async Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken) =>
            await FindByNameAsync(normalizedEmail, cancellationToken);

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            long id = long.Parse(userId);
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken) =>
            await _context.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedUserName, cancellationToken);

        public Task<string> GetEmailAsync(User user, CancellationToken cancellationToken) =>
            Task.FromResult(user.Email);

        public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken) =>
            GetNormalizedUserNameAsync(user, cancellationToken);

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken) =>
            Task.FromResult(user.NormalizedEmail);

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken) =>
            Task.FromResult(user.PasswordHash);

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken) =>
            Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));

        public Task SetEmailAsync(User user, string email, CancellationToken cancellationToken) =>
            SetUserNameAsync(user, email, cancellationToken);

        public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public async Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken) =>
            await SetNormalizedUserNameAsync(user, normalizedEmail, cancellationToken);

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            user.Email = userName;
            return Task.CompletedTask;
        }

        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
