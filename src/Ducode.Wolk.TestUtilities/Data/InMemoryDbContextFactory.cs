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

        public static void Destroy(WolkDbContext context)
        {
            try
            {
                context.Database.EnsureDeleted();
            }
            catch
            {
                // In integration test scenario, the framework might already have disposed the database.
                // So we don't care about exceptions here.
            }

            context.Dispose();
        }
    }
}
