using System;
using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Application.Interfaces.Identity;
using Ducode.Wolk.Configuration;
using Ducode.Wolk.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ducode.Wolk.Identity.Impl
{
    public class DefaultUserCreator : IDefaultUserCreator
    {
        private readonly ILogger<DefaultUserCreator> _logger;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRegistrationManager _registrationManager;
        private readonly IWolkDbContext _wolkDbContext;
        private readonly WolkConfiguration _configuration;

        public DefaultUserCreator(
            ILogger<DefaultUserCreator> logger,
            IPasswordHasher<User> passwordHasher,
            IRegistrationManager registrationManager,
            IWolkDbContext wolkDbContext,
            IOptions<WolkConfiguration> options)
        {
            _logger = logger;
            _passwordHasher = passwordHasher;
            _registrationManager = registrationManager;
            _wolkDbContext = wolkDbContext;
            _configuration = options.Value;
        }

        public async Task CreateOrUpdateDefaultUser()
        {
            var email = _configuration.DefaultLoginEmail;
            var password = _configuration.DefaultPassword;
            if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password))
            {
                _logger.LogInformation("Checking if a default user has to be created.");
                var user = await _wolkDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    _logger.LogInformation($"Creating a new default user with username '{email}'.");
                    await _registrationManager.RegisterUserAsync(email, password);
                }
                else if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password) ==
                         PasswordVerificationResult.Failed)
                {
                    _logger.LogInformation($"Default user with username '{email}' already created, but password has changed.");

                    user.PasswordHash = _passwordHasher.HashPassword(user, password);
                    user.SecurityStamp = Guid.NewGuid().ToString();
                    await _wolkDbContext.SaveChangesAsync(CancellationToken.None);
                }
            }
        }
    }
}
