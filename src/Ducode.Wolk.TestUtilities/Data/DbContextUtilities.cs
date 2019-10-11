using Ducode.Wolk.Persistence;

namespace Ducode.Wolk.TestUtilities.Data
{
    public static class DbContextUtilities
    {
        public static void Destroy(this WolkDbContext context)
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

            try
            {
                context.Dispose();
            }
            catch
            {
                // Same story.
            }
        }
    }
}
