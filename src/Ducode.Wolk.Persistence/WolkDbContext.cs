using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Domain.Entities;
using Ducode.Wolk.Persistence.SaveChanges;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.Persistence
{
    public class WolkDbContext : DbContext, IWolkDbContext
    {
        private readonly IEnumerable<ISaveChangesHandler> _saveChangesHandlers;

        public WolkDbContext(
            DbContextOptions<WolkDbContext> options,
            IEnumerable<ISaveChangesHandler> saveChangesHandlers = null) : base(options)
        {
            _saveChangesHandlers = saveChangesHandlers;
        }

        public DbSet<Attachment> Attachments { get; set; }

        public DbSet<Note> Notes { get; set; }

        public DbSet<Notebook> Notebooks { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<AccessToken> AccessTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WolkDbContext).Assembly);

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (_saveChangesHandlers != null)
            {
                foreach (var handler in _saveChangesHandlers)
                {
                    await handler.HandleSaveChangesAsync(this);
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
