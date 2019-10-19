using System;
using System.Collections.Generic;
using Ducode.Wolk.Persistence;
using Ducode.Wolk.Persistence.SaveChanges;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.TestUtilities.Data
{
    public static class InMemoryDbContextFactory
    {
        public static WolkDbContext Create(IEnumerable<ISaveChangesHandler> saveChangesHandlers = null)
        {
            var options = new DbContextOptionsBuilder<WolkDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new TestWolkDbContext(options, saveChangesHandlers);
            context.Database.EnsureCreated();
            return context;
        }
    }
}
