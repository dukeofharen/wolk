using System.Linq;
using System.Threading.Tasks;
using Ducode.Wolk.Common;
using Ducode.Wolk.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Ducode.Wolk.Persistence.SaveChanges.Impl
{
    internal class ChangeDateSaveChangesHandler : ISaveChangesHandler
    {
        private readonly IDateTime _dateTime;

        public ChangeDateSaveChangesHandler(IDateTime dateTime)
        {
            _dateTime = dateTime;
        }

        public Task HandleSaveChangesAsync(WolkDbContext context)
        {
            // Don't set Changed where the Changed was set manually.
            var entries = context.ChangeTracker
                .Entries()
                .Where(e =>
                    e.State == EntityState.Modified &&
                    e.Entity is BaseEntity &&
                    !ChangeDateIsUpdatedManually(e));
            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;
                entity.Changed = _dateTime.Now;
            }

            return Task.CompletedTask;
        }

        private bool ChangeDateIsUpdatedManually(EntityEntry entry)
        {
            var original = (BaseEntity)entry.OriginalValues.ToObject();
            var current = (BaseEntity)entry.CurrentValues.ToObject();
            return original.Changed != current.Changed;
        }
    }
}
