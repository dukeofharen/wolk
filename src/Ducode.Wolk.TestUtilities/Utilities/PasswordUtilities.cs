using Ducode.Wolk.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Ducode.Wolk.TestUtilities.Utilities
{
    public static class PasswordUtilities
    {
        public static string CreateDeprecatedPasswordHash(string input)
        {
            var options = Options.Create(new PasswordHasherOptions());
            options.Value.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2;
            var hasher = new PasswordHasher<User>(options);
            return hasher.HashPassword(new User(), input);
        }
    }
}
