using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.Application.Interfaces
{
    public interface IWolkDbContext
    {
        DbSet<Note> Notes { get; set; }

        DbSet<Notebook> Notebooks { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
