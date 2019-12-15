using Ducode.Wolk.Domain.Entities;
using Ducode.Wolk.TestUtilities.Config;
using Microsoft.AspNetCore.Identity;

namespace Ducode.Wolk.TestUtilities.Utilities
{
    public static class PasswordUtilities
    {
        public static string CreateDeprecatedPasswordHash(string input)
        {
            var options = MockOptions<PasswordHasherOptions>.Create(new PasswordHasherOptions());
            options.Value.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2;
            var hasher = new PasswordHasher<User>(options);
            return hasher.HashPassword(new User(), input);
        }
    }
}
