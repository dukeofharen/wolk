using Ducode.Wolk.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.TestUtilities.Data
{
    public class TestWolkDbContext : WolkDbContext
    {
        public TestWolkDbContext(DbContextOptions<WolkDbContext> options) : base(options)
        {
        }

        public override void Dispose()
        {
            // Intentionally left blank.
        }
    }
}
