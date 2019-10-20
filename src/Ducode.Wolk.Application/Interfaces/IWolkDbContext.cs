using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.Application.Interfaces
{
    public interface IWolkDbContext
    {
        DbSet<Attachment> Attachments { get; set; }

        DbSet<Note> Notes { get; set; }

        DbSet<Notebook> Notebooks { get; set; }

        DbSet<User> Users { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
