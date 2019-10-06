using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.Persistence
{
    public class WolkDbContextFactory : DesignTimeDbContextFactoryBase<WolkDbContext>
    {
        protected override WolkDbContext CreateNewInstance(DbContextOptions<WolkDbContext> options) =>
            new WolkDbContext(options);
    }
}
