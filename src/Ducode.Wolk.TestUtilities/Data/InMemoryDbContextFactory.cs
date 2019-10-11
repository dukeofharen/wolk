using System;
using Ducode.Wolk.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.TestUtilities.Data
{
    public static class InMemoryDbContextFactory
    {
        public static WolkDbContext Create()
        {
            var options = new DbContextOptionsBuilder<WolkDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new TestWolkDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}
