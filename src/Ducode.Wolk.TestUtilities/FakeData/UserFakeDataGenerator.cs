using System;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Ducode.Wolk.TestUtilities.FakeData
{
    public static class UserFakeDataGenerator
    {
        private static readonly Faker _faker = new Faker();
        private static readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public static User CreateUser()
        {
            var email = _faker.Internet.Email();
            var user = new User
            {
                Changed = _faker.Date.Past(),
                Created = _faker.Date.Past(),
                Email = email,
                NormalizedEmail = email.ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString()
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, "Pass123");
            return user;
        }

        public static async Task<User> CreateAndSaveUser(this IWolkDbContext wolkDbContext, Action<User> action = null)
        {
            var user = CreateUser();
            action?.Invoke(user);
            wolkDbContext.Users.Add(user);
            await wolkDbContext.SaveChangesAsync(CancellationToken.None);
            return user;
        }
    }
}
