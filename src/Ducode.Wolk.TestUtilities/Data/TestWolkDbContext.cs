using System.Collections.Generic;
using System.Threading.Tasks;
using Ducode.Wolk.Persistence;
using Ducode.Wolk.Persistence.SaveChanges;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.TestUtilities.Data
{
    public class TestWolkDbContext : WolkDbContext
    {
        public TestWolkDbContext(
            DbContextOptions<WolkDbContext> options,
            IEnumerable<ISaveChangesHandler> saveChangesHandlers = null) : base(options, saveChangesHandlers)
        {
        }

        public override void Dispose()
        {
            // Intentionally left blank...
        }

        public override ValueTask DisposeAsync() => new ValueTask();
    }
}
