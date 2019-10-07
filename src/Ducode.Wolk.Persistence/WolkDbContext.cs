using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.Persistence
{
    public class WolkDbContext : DbContext, IWolkDbContext
    {
        public WolkDbContext(DbContextOptions<WolkDbContext> options) : base(options)
        {
        }

        public DbSet<Note> Notes { get; set; }

        public DbSet<Notebook> Notebooks { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WolkDbContext).Assembly);
    }
}
